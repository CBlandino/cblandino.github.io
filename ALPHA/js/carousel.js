let currentIndex = 0; // Start from the first product
const products = document.querySelectorAll('.product-item');

function moveCarousel(direction) {
    // Update the current index based on the direction
    currentIndex = (currentIndex + direction + products.length) % products.length;

    // Update the active product
    updateActiveProduct();
}

function updateActiveProduct() {
    products.forEach((product, index) => {
        product.classList.remove('active');
        if (index === currentIndex) {
            product.classList.add('active');
        }
    });
}

// Automatically cycle through products every 3 seconds
setInterval(() => {
    moveCarousel(1); // Move to the next product
}, 3000);
