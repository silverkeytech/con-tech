function viewComponent() {

    // Initialize PDF.js
    pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/3.4.120/pdf.worker.min.js';

    let pdfDoc = null;
    let currentPage = 1;
    let currentScale = 1.0;
    const scaleIncrement = 0.25;
    const container = document.getElementById('svg_client_2_container');
    const closeModalButton = document.getElementById('btn-close');
    const openModalButton = document.getElementById('openUploadLevelButton');

    const tooltip2 = d3.select("#tooltip")
        .style("opacity", 0)
        .attr("class", "tooltip");

    let canvas = null;
    let ctx = null;


    // Keyboard shortcuts
    document.addEventListener('keydown', function (e) {
        if (e.ctrlKey) {
            if (e.key === '+' || e.key === '=') {
                zoomIn();
                e.preventDefault();
            } else if (e.key === '-') {
                zoomOut();
                e.preventDefault();
            } else if (e.key === '0') {
                zoomToFit();
                e.preventDefault();
            }
        }
    });
    const viewComponent = {
        viewList: [],
        isUploading: false,
        error: null,
        dxfFile: null,
        excelFile: null,
        isEditMode: false,
        progress: 2,
        isDragging: false,
        successMessage: '',
        errorMessage: '',
        currentLevels: [],
        viewId: 0,
        levelData: {
            realLevelId: '',
            id: '',
            viewId: 0,
            levelName: '',
            description: '',
            levelScale: '1',
            dxfFile: '',
            excelFile: '',
            transitionX: 0,
            transitionY: 0,

        },
        async updateViewLevelTransition(levelId) {

            var targetLevel = this.currentLevels.find(item => item.realLevelId == levelId);

            const metadata = {
                id: targetLevel.realLevelId,
                transitionX: String(targetLevel.transitionX),
                transitionY: String(targetLevel.transitionY),
            };

            await fetch('/admin/view/update-view-level-transition', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(metadata),
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`HTTP error! Status: ${response.status}`);
                    }
                    return response.json();
                })
                .then(data => {
                    console.log('Success:', data);
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        },
        async updateViewLevelXhrWay() {
            this.uploading = true;
            this.progress = 0;

            const formData = new FormData();

            // Add JSON metadata as a part
            const metadata = {
                id: this.levelData.realLevelId,
                levelName: this.levelData.levelName,
                levelScale: String(this.levelData.levelScale),
                transitionX: String(this.levelData.transitionX),
                transitionY: String(this.levelData.transitionY),
                fileInfo: [],
            };
            formData.append('metadata', JSON.stringify(metadata));

            //// Add files as separate parts
            //this.files.forEach(file => {
            //    formData.append('files', file, file.name);
            //});

            formData.append('dxfFile', this.dxfFile);
            let dxfFileInfo = {
                originalName: this.dxfFile.name,
                size: this.dxfFile.size,
                type: this.dxfFile.type
            }
            metadata.fileInfo.push(dxfFileInfo);

            formData.append('excelFile', this.excelFile);
            let excelFileInfo = {
                originalName: this.excelFile.name,
                size: this.excelFile.size,
                type: this.excelFile.type
            }
            metadata.fileInfo.push(excelFileInfo);


            try {
                const xhr = new XMLHttpRequest();

                xhr.upload.addEventListener('progress', (e) => {
                    if (e.lengthComputable) {
                        this.progress = Math.round((e.loaded / e.total) * 100);
                    }
                });

                const response = await new Promise((resolve, reject) => {
                    xhr.onreadystatechange = () => {
                        if (xhr.readyState === 4) {
                            if (xhr.status === 200) {
                                resolve(JSON.parse(xhr.responseText));
                            } else {
                                reject(new Error('Upload failed'));
                            }
                        }
                    };

                    xhr.open('POST', '/admin/view/update-view-level', true);
                    xhr.send(formData);
                });
                //await this.fetchViewDetails(this.viewId);
                console.log('Upload successful:', response);
            } catch (error) {
                console.error('Upload error:', error);
            } finally {
                this.uploading = false;

                await this.fetchViewDetails(this.viewId);
                this.isEditMode = false;
                closeModalButton.click();
            }
        },
        async removeLevel(levelId) {
            try {
                if (levelId) {
                    const response = await fetch('/admin/view/disable-view-level/' + levelId, {
                        method: 'POST', // <-- Explicitly set to POST
                        headers: {
                            'Content-Type': 'application/json', // (Optional but recommended)
                        }
                    });
                    view = await response.json();
                    this.error = null;
                }
            } catch (err) {
                this.error = 'Failed to load view';
                console.error(err);
            } finally {
                this.loading = false;
            }

            d3.select("svg").selectAll("#_" + levelId).remove();  // Remove old DXF group
        },
        bindLevelForEdit(levelId) {

            var targetLevel = this.currentLevels.find(item => item.realLevelId == levelId);
            if (targetLevel) {

                this.levelData.levelName = targetLevel.levelName;
                this.levelData.realLevelId = targetLevel.realLevelId;
                this.levelData.levelScale = targetLevel.levelScale;
                this.levelData.transitionX = targetLevel.transitionX;
                this.levelData.transitionY = targetLevel.transitionY;
                this.isEditMode = true;
                openModalButton.click();
            }
        },
        showHideLevel(e, levelId) {
            const isChecked = e.target.checked;
            const layerGroup = d3.select("svg").selectAll("#" + levelId);
            layerGroup.style('display', isChecked ? null : 'none');
        },
        async fetchViewDetails(id) {
            this.viewId = id;
            canvas = document.getElementById('pdf-canvas-' + id);
            ctx = canvas.getContext('2d');

            try {

                this.loading = true;
                var view = this.viewList.find(item => item.id == id);

                if (!view) {

                    const response = await fetch('/admin/view/get-view-details-by-id/' + id);
                    view = await response.json();

                    this.viewList.push(view);
                    view.viewLevels.forEach(level => {
                        var newLevel = {
                            description: level.description,
                            id: '_' + level.id,
                            realLevelId: level.id,
                            dxfFile: level.dxfFile,
                            dxfData: this.parseDxfFromBase64(level.dxfFile),
                            excelFile: level.excelFile,
                            excelData: this.parseExcelFromBase64(level.excelFile),
                            levelName: level.name,
                            levelScale: level.scale,
                            transitionX: level.transitionX,
                            transitionY: level.transitionY,
                            viewId: id,
                        };
                        this.currentLevels.push(newLevel);
                        this.drawUploadedDXF(newLevel);

                        d3.select("svg").selectAll("#" + newLevel.id).attr("transform", `translate(${newLevel.transitionX},${newLevel.transitionY})`);
                    });

                    console.log(this.currentLevels);
                }

                await this.base64ToUint8Array(view.backgroundPdf);

                this.error = null;

            } catch (err) {
                this.error = 'Failed to load view';
                console.error(err);
            } finally {
                this.loading = false;
            }
        },

        async base64ToUint8Array(base64) {
            const binaryString = atob(base64);
            const pdfData = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                pdfData[i] = binaryString.charCodeAt(i);
            }

            const loadingTask = pdfjsLib.getDocument({ data: pdfData });
            pdfDoc = await loadingTask.promise;

            this.renderPage(currentPage, currentScale);

        },

        renderPage(pageNum, scale) {
            pdfDoc.getPage(pageNum).then(function (page) {
                const viewport = page.getViewport({ scale: scale });

                canvas.height = viewport.height;
                canvas.width = viewport.width;

                container.style.width = `${viewport.width}px`;
                container.style.height = `${viewport.height}px`;

                const renderContext = {
                    canvasContext: ctx,
                    viewport: viewport
                };

                page.render(renderContext);
            });
            this.updateZoomDisplay();
        },
        zoomIn() {
            currentScale += scaleIncrement;
            this.renderPage(currentPage, currentScale);
        },
        zoomOut() {
            if (currentScale > scaleIncrement) {
                currentScale -= scaleIncrement;
                this.renderPage(currentPage, currentScale);
            }
        },
        zoomToFit() {
            const containerWidth = container.clientWidth - 40; // Account for padding
            pdfDoc.getPage(currentPage).then(function (page) {
                const pageWidth = page.getViewport({ scale: 1.0 }).width;
                currentScale = containerWidth / pageWidth;
            });
            this.renderPage(currentPage, currentScale);
        },
        updateZoomDisplay() {
            document.getElementById('zoom-level').textContent = `${Math.round(currentScale * 100)}%`;
        },
        //async uploadFilesFetchWay() {

        //    this.uploading = true;
        //    this.progress = 0;

        //    const formData = new FormData();

        //    // Add JSON metadata as a part
        //    const metadata = {
        //        levelId: this.levelData.levelId,
        //        levelName: this.levelData.levelName,
        //        levelScale: this.levelData.levelScale,
        //        fileInfo: this.files.map(file => ({
        //            originalName: file.name,
        //            size: file.size,
        //            type: file.type
        //        }))
        //    };
        //    formData.append('metadata', JSON.stringify(metadata));

        //    // Add files as separate parts
        //    this.files.forEach(file => {
        //        formData.append('files', file, file.name);
        //    });

        //    const response = await fetch('/admin/view/add-view-level', {
        //        method: 'POST',
        //        body: formData,
        //        signal: this.abortController?.signal
        //    });

        //    // For progress tracking with fetch:
        //    const reader = response.body.getReader();
        //    const contentLength = +response.headers.get('Content-Length');
        //    let receivedLength = 0;
        //    let chunks = [];

        //    while (true) {
        //        const { done, value } = await reader.read();
        //        if (done) break;
        //        chunks.push(value);
        //        receivedLength += value.length;
        //        this.progress = Math.round((receivedLength / contentLength) * 100);
        //    }

        //    // Process complete response
        //    const chunksAll = new Uint8Array(receivedLength);
        //    let position = 0;
        //    for (let chunk of chunks) {
        //        chunksAll.set(chunk, position);
        //        position += chunk.length;
        //    }

        //    const result = new TextDecoder("utf-8").decode(chunksAll);
        //    return JSON.parse(result);
        //},


        async uploadFilesXhrWay() {
            this.uploading = true;
            this.progress = 0;

            const formData = new FormData();

            // Add JSON metadata as a part
            const metadata = {
                viewId: this.levelData.viewId,
                levelName: this.levelData.levelName,
                levelScale: this.levelData.levelScale,
                fileInfo: [],
            };
            formData.append('metadata', JSON.stringify(metadata));

            //// Add files as separate parts
            //this.files.forEach(file => {
            //    formData.append('files', file, file.name);
            //});

            formData.append('dxfFile', this.dxfFile);
            let dxfFileInfo = {
                originalName: this.dxfFile.name,
                size: this.dxfFile.size,
                type: this.dxfFile.type
            }
            metadata.fileInfo.push(dxfFileInfo);

            formData.append('excelFile', this.excelFile);
            let excelFileInfo = {
                originalName: this.excelFile.name,
                size: this.excelFile.size,
                type: this.excelFile.type
            }
            metadata.fileInfo.push(excelFileInfo);


            try {
                const xhr = new XMLHttpRequest();

                xhr.upload.addEventListener('progress', (e) => {
                    if (e.lengthComputable) {
                        this.progress = Math.round((e.loaded / e.total) * 100);
                    }
                });

                const response = await new Promise((resolve, reject) => {
                    xhr.onreadystatechange = () => {
                        if (xhr.readyState === 4) {
                            if (xhr.status === 200) {
                                resolve(JSON.parse(xhr.responseText));
                            } else {
                                reject(new Error('Upload failed'));
                            }
                        }
                    };

                    xhr.open('POST', '/admin/view/add-view-level', true);
                    xhr.send(formData);
                });
                console.log('Upload successful:', response);
            } catch (error) {
                console.error('Upload error:', error);
            } finally {
                this.uploading = false;
                await this.fetchViewDetails(this.viewId);

                closeModalButton.click();
            }
        },
        parseDxfFromBase64(base64String) {
            // Convert Base64 to text (DXF content)
            const dxfText = atob(base64String);
            const parser = new DxfParser();

            try {
                const dxfData = parser.parseSync(dxfText);
                window.lastDxfData = dxfData; // Store dxfData globally
                console.log("DXF Parsed Data:", dxfData);
                return dxfData;
            } catch (error) {
                console.error("Error parsing DXF:", error);
                throw error; // Re-throw if you want calling code to handle it
            }
        }, parseDxfFromBase64_2(base64String) {
            // More robust Base64 to text conversion for UTF-8
            const binaryString = atob(base64String);
            const bytes = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }
            const dxfText = new TextDecoder().decode(bytes);

            // Rest of the parsing remains the same
            const parser = new DxfParser();
            try {
                const dxfData = parser.parseSync(dxfText);
                window.lastDxfData = dxfData;
                console.log("DXF Parsed Data:", dxfData);
                return dxfData;
            } catch (error) {
                console.error("Error parsing DXF:", error);
                throw error;
            }
        },

        addDxfFile(e) {
            this.dxfFile = e.target.files[0];

            if (!this.dxfFile)
                return console.log("No file selected");

            const reader = new FileReader();
            reader.onload = function (file) {
                const parser = new DxfParser();
                let dxfData;
                try {
                    dxfData = parser.parseSync(file.target.result);
                    window.lastDxfData = dxfData;

                } catch (error) {
                    console.error("Error parsing DXF:", error);
                    return;
                }
                console.log("DXF Parsed Data:", dxfData);
            };
            reader.readAsText(this.dxfFile);
        },

        addExcelFile(e) {
            this.excelFile = e.target.files[0];

            if (!this.excelFile)
                return console.log("No Excel selected");

            const reader = new FileReader();
            reader.onload = function (file) {
                const data = new Uint8Array(file.target.result);
                const workbook = XLSX.read(data, { type: 'array' });
                const sheetName = workbook.SheetNames[0];
                const sheet = workbook.Sheets[sheetName];
                excelData = XLSX.utils.sheet_to_json(sheet);
                console.log("Excel Parsed Data:", excelData);
                window.lastExcelData = excelData;
            };
            reader.readAsArrayBuffer(this.excelFile);
        },
        parseExcelFromBase64(base64String) {
            try {
                const base64WithoutPrefix = base64String.split(',')[1] || base64String;
                const binaryString = atob(base64WithoutPrefix);

                const bytes = new Uint8Array(binaryString.length);
                for (let i = 0; i < binaryString.length; i++) {
                    bytes[i] = binaryString.charCodeAt(i);
                }

                const workbook = XLSX.read(bytes, { type: 'array' });
                const sheetName = workbook.SheetNames[0];
                const sheet = workbook.Sheets[sheetName];
                const excelData = XLSX.utils.sheet_to_json(sheet);

                window.lastExcelData = excelData;
                return excelData;
            } catch (error) {
                console.error("Error parsing Excel:", error);
                throw error; // Or handle it as needed
            }
        },

        addFiles(e) {
            var newFiles = Array.from(e.target.files);
            // Filter out duplicates by name and size
            newFiles = newFiles.filter(newFile =>
                !this.files.some(existingFile =>
                    existingFile.name === newFile.name &&
                    existingFile.size === newFile.size
                )
            );

            this.files = [...this.files, ...newFiles];
        },
        getViewLevels() { },
        init() {
            this.getViewLevels();
        },
        drawUploadedDXF(levelData) {

            let dxfData = levelData.dxfData;
            let layersDetails = new Map(Object.entries(dxfData.tables.layer.layers).map(([key, value]) => [key.toLowerCase(), value])
            );
            let excelData = levelData.excelData;
            let levelId = levelData.id;
            let realLevelId = levelData.realLevelId;
            let userScale = levelData.levelScale;
            let levelName = levelData.levelName;
            const progressChecked = document.getElementById("flexSwitchCheckChecked").checked;

            const svgWidth = 1300;
            const svgHeight = 1300;

            const svg = d3.select("svg");
            svg.selectAll(`#${levelId}`).remove();  // Remove old DXF group
            const uploadGroup = svg.append("g")
                .attr("id", levelId)
                .attr("transform", "translate(0,0)")
                .style("cursor", "move");
            let currentX = 0, currentY = 0;
            // Make the group draggable
            uploadGroup.call(
                d3.drag()
                    .on("drag", function (event) {
                        currentX += event.dx;
                        currentY += event.dy;
                        d3.select(this).attr("transform", `translate(${currentX},${currentY})`);

                        if (viewComponent.currentLevels) {
                            viewComponent.currentLevels = viewComponent.currentLevels.map((level) => {
                                if (level.realLevelId === realLevelId) {
                                    level.transitionX = currentX;
                                    level.transitionY = currentY;
                                    return level;
                                }
                                return level;
                            });
                        }
                    })
                    .on("end", async function () {
                        await viewComponent.updateViewLevelTransition(realLevelId);
                    })
            );

            const bounds = this.getBounds(dxfData.entities);
            if (bounds.maxX === bounds.minX || bounds.maxY === bounds.minY) {
                console.error("Invalid bounds – DXF data may be empty or invalid");
                return;
            }

            const tooltip = d3.select("#tooltip");
            const baseScale = ((svgWidth + svgHeight) / 4) / (bounds.maxX - bounds.minX);
            const scale = baseScale * userScale;
            const offsetX = -bounds.minX * scale + (svgWidth - (bounds.maxX - bounds.minX) * scale) / 2;
            const offsetY = -bounds.minY * scale + (svgHeight - (bounds.maxY - bounds.minY) * scale) / 2;

            dxfData.entities.forEach(entity => {

                let entityColor = layersDetails.get(entity.layer.toLowerCase());
                const hexColor = `#${entityColor.color.toString(16).padStart(6, '0')}`;


                switch (entity.type) {
                    case "LINE":
                        uploadGroup.append("line")
                            .attr("x1", entity.vertices[0].x * scale + offsetX)
                            .attr("y1", svgHeight - (entity.vertices[0].y * scale + offsetY))
                            .attr("x2", entity.vertices[1].x * scale + offsetX)
                            .attr("y2", svgHeight - (entity.vertices[1].y * scale + offsetY))
                            .attr("stroke", "black")
                            .attr("stroke-width", 2)
                            .on("mouseover", function (event) {
                                d3.select("#tooltip")
                                    .style("display", "block")
                                    .style("opacity", 1)
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px")
                                    .html(getTooltipContent(entity) + "<br>Tooltip working");
                            })
                            .on("mousemove", function (event) {
                                d3.select("#tooltip")
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px");
                            })
                            .on("mouseout", function (d) {
                                tooltip.transition()
                                    .duration(500)
                                    .style("display", "none")
                                    .style("opacity", 0);
                            });
                        break;

                    case "CIRCLE":
                        uploadGroup.append("circle")
                            .attr("cx", entity.center.x * scale + offsetX)
                            .attr("cy", svgHeight - (entity.center.y * scale + offsetY))
                            .attr("r", entity.radius * scale)
                            .attr("stroke", "black")
                            .attr("fill", "none")
                            .attr("stroke-width", 2)
                            .on("mouseover", function (event) {
                                d3.select("#tooltip")
                                    .style("display", "block")
                                    .style("opacity", 1)
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px")
                                    .html(getTooltipContent(entity) + "<br>Tooltip working");
                            })
                            .on("mousemove", function (event) {
                                d3.select("#tooltip")
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px");
                            })
                            .on("mouseout", function (d) {
                                tooltip.transition()
                                    .duration(500)
                                    .style("display", "none")
                                    .style("opacity", 0);
                            });
                        break;

                    case "POLYLINE":
                    case "LWPOLYLINE":
                        const points = entity.vertices.map(v => [
                            v.x * scale + offsetX,
                            svgHeight - (v.y * scale + offsetY)
                        ]);
                        // const points = entity.vertices.map(v => [
                        //   v.x * 30,
                        //   (v.y * 30)
                        // ]);
                        uploadGroup.append("polyline")
                            .attr("points", points.join(" "))
                            .attr("stroke", "black")
                            .attr("stroke-width", 2)
                            .attr("name", `${levelId}`)
                            .attr("class", `area`)
                            .attr("data-level", levelId)
                            .attr("data-layer", entity.layer)
                            .attr("id", entity.layer)
                            .attr("fill", progressChecked ? getColorByCompleteness(parseFloat(excelData.find((e) => e.UniqueID == entity.layer)?.complete)) : `${hexColor}`)
                            .style("fill-opacity", 0.3)
                            .attr("stroke-width", 2)
                            //.call(getArea)
                            .on("mouseover", function (d) {
                                tooltip2
                                    .style("opacity", 1)
                                    .style("display", "block")
                                    .html(`
            <h3 class="layerName">${levelName}</h3>
            <div class="row">
              <div class="label">Complete:</div>
           <div class="value d-flex align-items-center">
            <span class="precentage">${Math.floor((excelData.find((e) => e.UniqueID == entity.layer)?.complete) * 100)}%</span>
            <div class="progress ml-3" id="smallProgress">
              <div class="progress-bar bg-success" role="progressbar" style="width: ${Math.floor((excelData.find((e) => e.UniqueID == entity.layer)?.complete) * 100)}%;" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100"></div>
              <div class="progress-bar bg-secondary" role="progressbar" style="width: ${100 - Math.floor((excelData.find((e) => e.UniqueID == entity.layer)?.complete) * 100)}%;" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100"></div>
            </div>
          </div>
             </div>
            <div class="row">
              <div class="label">Level:</div>
              <div class="value">${excelData.find((e) => e.UniqueID == entity.layer)?.Level}</div>
            </div>
            <div class="row">
              <div class="label">Finish:</div>
              <div class="value">${excelData.find((e) => e.UniqueID == entity.layer)?.Finish}</div>
            </div>
            <div class="row description-row">
              <div class="label">Description:</div>
              <div class="value" style="max-width: 400px;">${excelData.find((e) => e.UniqueID == entity.layer)?.Description}</div>
            </div>
            <div class="row">
              <div class="alert alert-warning" role="alert">
                 <i class="fas fa-exclamation-triangle"></i> Notes : Discoloration on the north wall
            </div>
            </div>
          `)
                                    .style("left", (event.pageX + 20) + "px")
                                    .style("top", (event.pageY - 30) + "px");
                            })
                            .on("mouseout", function () {
                                tooltip2
                                    .style("display", "none")
                                    .style("opacity", 0);
                            })
                            .on("mousemove", function (event) {
                                d3.select("#tooltip")
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px");
                            });
                        break;

                    case "POINT":
                        uploadGroup.append("circle")
                            .attr("cx", entity.position.x * scale + offsetX)
                            .attr("cy", svgHeight - (entity.position.y * scale + offsetY))
                            .attr("r", 3)
                            .attr("fill", "red")
                            .on("mouseover", function (event) {
                                d3.select("#tooltip")
                                    .style("display", "block")
                                    .style("opacity", 1)
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px")
                                    .html(getTooltipContent(entity) + "<br>Tooltip working");
                            })
                            .on("mousemove", function (event) {
                                d3.select("#tooltip")
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px");
                            })
                            .on("mouseout", function (d) {
                                tooltip.transition()
                                    .duration(500)
                                    .style("display", "none")
                                    .style("opacity", 0);
                            });
                        break;

                    case "MTEXT":
                        uploadGroup.append("text")
                            .attr("x", entity.position.x * scale + offsetX)
                            .attr("y", svgHeight - (entity.position.y * scale + offsetY))
                            .attr("font-size", 12)
                            .attr("fill", "blue")
                            .text(entity.text || "")
                            .on("mouseover", function (event) {
                                d3.select("#tooltip")
                                    .style("display", "block")
                                    .style("opacity", 1)
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px")
                                    .html(getTooltipContent(entity) + "<br>Tooltip working");
                            })
                            .on("mousemove", function (event) {
                                d3.select("#tooltip")
                                    .style("left", (event.pageX + 10) + "px")
                                    .style("top", (event.pageY + 10) + "px");
                            })
                            .on("mouseout", function (d) {
                                tooltip.transition()
                                    .duration(500)
                                    .style("display", "none")
                                    .style("opacity", 0);
                            });
                        break;

                    default:
                        console.log("Unsupported uploaded entity type:", entity.type);
                }
            });
        },
        getBounds(entities) {
            let minX = Infinity, minY = Infinity, maxX = -Infinity, maxY = -Infinity;
            entities.forEach(entity => {
                if (entity.vertices) {
                    entity.vertices.forEach(vertex => {
                        minX = Math.min(minX, vertex.x);
                        minY = Math.min(minY, vertex.y);
                        maxX = Math.max(maxX, vertex.x);
                        maxY = Math.max(maxY, vertex.y);
                    });
                } else if (entity.center) {
                    minX = Math.min(minX, entity.center.x - entity.radius);
                    minY = Math.min(minY, entity.center.y - entity.radius);
                    maxX = Math.max(maxX, entity.center.x + entity.radius);
                    maxY = Math.max(maxY, entity.center.y + entity.radius);
                }
            });
            return { minX, minY, maxX, maxY };
        },
        getColorByCompleteness(percentage) {
            //console.log("the precentage ",percentage);
            if (percentage <= 0.29) return "#DB4E41";      // Red
            if (percentage <= 0.70) return "#ECCB28";      // Yellow
            return "#97CA58";                            // Green
        },
        selectedEntities: [],
        bindLevelChilds(levelId) {

            var targetLevel = this.currentLevels.find(item => item.realLevelId == levelId);

            if (targetLevel) {
                this.selectedEntities = targetLevel.dxfData.entities;
                //Object.entries(targetLevel.dxfData.entities).forEach(([index, entity]) => {

                //    const li = document.createElement('li');
                //    li.classList.add('d-flex', 'align-items-center', 'mb-2');
                //    li.setAttribute('name', childLevel_uuid);

                //    // Create checkbox
                //    const checkbox = document.createElement('input');
                //    checkbox.type = 'checkbox';
                //    checkbox.classList.add('form-check-input', 'me-2');
                //    checkbox.checked = false;
                //    checkbox.setAttribute('name', childLevel_uuid);
                //    checkbox.setAttribute('data-areaId', entity.layer);


                //    // Create layer name text
                //    const span = document.createElement('span');
                //    span.textContent = entity.layer;
                //    span.classList.add('me-auto');
                //    span.setAttribute('name', childLevel_uuid);


                //    li.appendChild(checkbox);
                //    li.appendChild(span);
                //    //modalList.append(li);

                //});
            }
        },
    };
    return viewComponent;
}