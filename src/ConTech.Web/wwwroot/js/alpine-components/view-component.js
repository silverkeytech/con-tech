function viewComponent() {

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
        error: null,
        newProduct: { name: '', price: 0, inStock: false },

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
        }
        //async addProduct() {
        //    try {
        //        const response = await fetch('/api/products', {
        //            method: 'POST',
        //            headers: {
        //                'Content-Type': 'application/json'
        //            },
        //            body: JSON.stringify(this.newProduct)
        //        });

        //        if (response.ok) {
        //            await this.fetchProducts();
        //            this.newProduct = { name: '', price: 0, inStock: false };
        //        }
        //    } catch (err) {
        //        console.error('Failed to add product:', err);
        //    }
        //}
    };

}