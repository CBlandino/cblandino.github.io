function showMessage(index) {
    // Hide all messages and buttons first
    let messages = document.querySelectorAll('.message');
    let buttons = document.querySelectorAll('button');
    messages.forEach(message => {
        message.style.display = 'none';
    });
    buttons.forEach(button => {
        button.style.display = 'none';
    });

    // Show the selected message
    document.getElementById(`message-${index}`).style.display = 'block';

    // Show the next button if available
    if (index < messages.length - 1) {
        buttons[index + 1].style.display = 'block';
    }
}
