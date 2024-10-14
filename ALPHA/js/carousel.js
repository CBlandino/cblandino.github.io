let currentIndex = 1; // Start at 1 to show the first product in the center
const productGrid = document.querySelector('.product-grid');
const productItems = document.querySelectorAll('.product-item');
const totalProducts = productItems.length;

// Clone the first few products for infinite scrolling
const clonedProducts = Array.from(productItems).map(item => {
    const clonedItem = item.cloneNode(true);
    productGrid.appendChild(clonedItem); // Append cloned item to the grid
    return clonedItem;
});

// Function to cycle through products
function cycleProducts() {
    // Remove the active class from all products
    productItems.forEach((item, index) => {
        item.classList.remove('active');
        item.style.transform = 'translateY(0)'; // Reset Y position
        item.style.transition = 'transform 0.5s ease, transform 0.5s ease'; // Reset transition
        if (index !== currentIndex) {
            item.style.transform = 'scale(0.8)'; // Scale down non-active items
        }
    });

    // Set the active class on the current product
    productItems[currentIndex].classList.add('active');
    
    // Bring the active item up and enlarge it
    productItems[currentIndex].style.transform = 'translateY(-30px) scale(1.1)'; // Move up and enlarge

    // Calculate the translateX value based on the currentIndex
    const translateX = -currentIndex * (250 + 20); // Width + margin for proper spacing
    productGrid.style.transform = `translateX(${translateX}px)`; // Move the product grid

    // If currentIndex reaches the last product, reset to first
    if (currentIndex === totalProducts) {
        currentIndex = 1; // Reset to the first original product
        productGrid.style.transition = 'none'; // Disable transition for instant jump
        productGrid.style.transform = `translateX(-${(currentIndex) * (250 + 20)}px)`; // Jump back to first product
        setTimeout(() => {
            productGrid.style.transition = 'transform 0.5s ease'; // Re-enable transition
        }, 50); // Small delay to allow jump to take effect
    } else {
        currentIndex++; // Move to the next product
    }
}

// Automatically cycle through the products every 5 seconds
setInterval(cycleProducts, 5000);

// Set the first product as active initially
productItems[currentIndex].classList.add('active');
productItems[currentIndex].style.transform = 'translateY(-30px) scale(1.1)'; // Initial position for the first product
