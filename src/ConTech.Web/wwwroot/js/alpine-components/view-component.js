function viewComponent(viewId = 0) {

    // Initialize PDF.js
    pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/3.4.120/pdf.worker.min.js';

    let pdfDoc = null;
    let currentPage = 1;
    let currentScale = 1.0;
    const scaleIncrement = 0.25;
    const container = document.getElementById('svg_client_2_container');
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

    return {
        viewList: [],
        loading: true,
        isUploading: false,
        error: null,
        dxfFile: null,
        excelFile: null,
        uploading: false,
        progress: 10,
        authorName: '',
        isDragging: false,
        uploadProgress: 0,
        successMessage: '',
        errorMessage: '',
        newProduct: { name: '', price: 0, inStock: false },
        formData: {
            author: '',
            attachments: []
        },
        levelData: {
            viewId: viewId,
            levelName: '',
            levelScale: 1,
        },

        async fetchViewDetails(id) {

            canvas = document.getElementById('pdf-canvas-' + id);
            ctx = canvas.getContext('2d');

            try {

                this.loading = true;
                var view = this.viewList.find(item => item.id == id);

                if (!view) {

                    const response = await fetch('/admin/view/get-view-details-by-id/' + id);
                    view = await response.json();

                    this.viewList.push(view);
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
        /* add business */
        async addLevel() {
            try {
                const response = await fetch('/admin/view/add-view-level/', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.newProduct)
                });

                if (response.ok) {
                    await this.fetchProducts();
                    this.newProduct = { name: '', price: 0, inStock: false };
                }
            } catch (err) {
                console.error('Failed to add product:', err);
            }
        },
        async uploadFilesFetchWay() {
            
            this.uploading = true;
            this.progress = 0;

            const formData = new FormData();

            // Add JSON metadata as a part
            const metadata = {
                levelId: this.levelData.levelId,
                levelName: this.levelData.levelName,
                levelScale: this.levelData.levelScale,
                fileInfo: this.files.map(file => ({
                    originalName: file.name,
                    size: file.size,
                    type: file.type
                }))
            };
            formData.append('metadata', JSON.stringify(metadata));

            // Add files as separate parts
            this.files.forEach(file => {
                formData.append('files', file, file.name);
            });

            const response = await fetch('/admin/view/add-view-level', {
                method: 'POST',
                body: formData,
                signal: this.abortController?.signal
            });

            // For progress tracking with fetch:
            const reader = response.body.getReader();
            const contentLength = +response.headers.get('Content-Length');
            let receivedLength = 0;
            let chunks = [];

            while (true) {
                const { done, value } = await reader.read();
                if (done) break;
                chunks.push(value);
                receivedLength += value.length;
                this.progress = Math.round((receivedLength / contentLength) * 100);
            }

            // Process complete response
            const chunksAll = new Uint8Array(receivedLength);
            let position = 0;
            for (let chunk of chunks) {
                chunksAll.set(chunk, position);
                position += chunk.length;
            }

            const result = new TextDecoder("utf-8").decode(chunksAll);
            return JSON.parse(result);
        },


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
            }
        },

        //handleFileSelect(event) {
        //    this.addFiles(Array.from(event.target.files));
        //    event.target.value = ''; // Reset input to allow selecting same files again
        //},

        //handleDrop(event) {
        //    this.isDragging = false;
        //    this.addFiles(Array.from(event.dataTransfer.files));
        //},

        addDxfFile(e) {
            this.dxfFile = e.target.files[0];
        },

        addExcelFile(e) {
            this.excelFile = e.target.files[0];
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

        //removeFile(index) {
        //    this.files.splice(index, 1);
        //},

        //formatFileSize(bytes) {
        //    if (bytes === 0) return '0 Bytes';
        //    const k = 1024;
        //    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        //    const i = Math.floor(Math.log(bytes) / Math.log(k));
        //    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
        //},

        //async uploadFiles1() {
        //    if (this.files.length === 0) {
        //        this.errorMessage = 'Please select at least one file';
        //        return;
        //    }

        //    if (!this.levelData.levelName.trim()) {
        //        this.errorMessage = 'Please enter level name';
        //        return;
        //    }

        //    this.isUploading = true;
        //    this.uploadProgress = 0;
        //    this.successMessage = '';
        //    this.errorMessage = '';

        //    try {
        //        const formData = new FormData();
        //        formData.append('authorName', this.authorName);

        //        this.files.forEach(file => {
        //            formData.append('files', file);
        //        });

        //        const xhr = new XMLHttpRequest();

        //        xhr.upload.addEventListener('progress', (event) => {
        //            if (event.lengthComputable) {
        //                this.uploadProgress = Math.round((event.loaded / event.total) * 100);
        //            }
        //        });

        //        const response = await new Promise((resolve, reject) => {
        //            xhr.onreadystatechange = () => {
        //                if (xhr.readyState === 4) {
        //                    if (xhr.status === 200) {
        //                        resolve(JSON.parse(xhr.responseText));
        //                    } else {
        //                        reject(xhr.responseText ? JSON.parse(xhr.responseText) : 'Upload failed');
        //                    }
        //                }
        //            };

        //            xhr.open('POST', '/api/upload', true);
        //            xhr.send(formData);
        //        });

        //        this.successMessage = `Success! ${response.uploadCount} file(s) uploaded by ${response.Author}`;
        //        console.log('Upload response:', response);
        //        this.files = [];
        //        this.$refs.uploadForm.reset();
        //    } catch (error) {
        //        console.error('Upload error:', error);
        //        this.errorMessage = error.message || 'An error occurred during upload';
        //    } finally {
        //        this.isUploading = false;
        //        this.uploadProgress = 0;
        //    }
        //},
    };

}