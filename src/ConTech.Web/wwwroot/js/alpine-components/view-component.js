 function viewComponent() {
    return {
        view: {},
        loading: true,
        error: null,
        newProduct: { name: '', price: 0, inStock: false },

        async fetchViewDetails(id) {
            try {
                debugger
                this.loading = true;
                const response = await fetch('/admin/view/get-view-details-by-id/'+id);
                this.view = await response.json();
                this.error = null;
            } catch (err) {
                this.error = 'Failed to load view';
                console.error(err);
            } finally {
                this.loading = false;
            }
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
}