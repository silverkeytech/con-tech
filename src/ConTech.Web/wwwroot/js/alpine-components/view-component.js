function viewComponent() {

    // Initialize PDF.js
    pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/3.4.120/pdf.worker.min.js';

    let pdfDoc = null;
    let currentPage = 1;
    let currentScale = 1.0;
    const scaleIncrement = 0.25;
    const container = document.getElementById('svg_client_2_container');
    const canvas = document.getElementById('pdf-canvas');
    const ctx = canvas.getContext('2d');


    return {
        view: {},
        loading: true,
        error: null,
        newProduct: { name: '', price: 0, inStock: false },

        async fetchViewDetails(id) {
            try {
                
                this.loading = true;
                const response = await fetch('/admin/view/get-view-details-by-id/' + id);
                this.view = await response.json();

                await this.base64ToUint8Array(this.view.backgroundPdf);

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
                updateZoomDisplay();
            });
        },
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

    function zoomIn() {
        currentScale += scaleIncrement;
        renderPage(currentPage, currentScale);
    }

    function zoomOut() {
        if (currentScale > scaleIncrement) {
            currentScale -= scaleIncrement;
            renderPage(currentPage, currentScale);
        }
    }

    function zoomToFit() {
        const containerWidth = container.clientWidth - 40; // Account for padding
        pdfDoc.getPage(currentPage).then(function (page) {
            const pageWidth = page.getViewport({ scale: 1.0 }).width;
            currentScale = containerWidth / pageWidth;
            renderPage(currentPage, currentScale);
        });
    }

    function updateZoomDisplay() {
        document.getElementById('zoom-level').textContent = `${Math.round(currentScale * 100)}%`;
    }

    // Event listeners
    document.getElementById('zoom-in').addEventListener('click', zoomIn);
    document.getElementById('zoom-out').addEventListener('click', zoomOut);
    document.getElementById('zoom-fit').addEventListener('click', zoomToFit);

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
}