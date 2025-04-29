document.addEventListener('DOMContentLoaded', function() {
    const terminal = document.getElementById('terminal');
    const commands = [
      { text: "christopher@portfolio:~$ git clone https://github.com/cblandino/portfolio", delay: 800 },
      { text: "Cloning repository...", delay: 1000 },
      { text: "✔ Repository cloned successfully", delay: 500 },
      { text: "christopher@portfolio:~$ cd portfolio", delay: 500 },
      { text: "christopher@portfolio:~$ npm install", delay: 800 },
      { text: "Installing dependencies...", delay: 1200 },
      { text: "✔ React", delay: 200 },
      { text: "✔ Node.js", delay: 200 },
      { text: "✔ Flutter", delay: 200 },
      { text: "✔ Golang", delay: 200 },
      { text: "christopher@portfolio:~$ flutter build web", delay: 1000 },
      { text: "Building Flutter application...", delay: 1500 },
      { text: "✔ Web build completed", delay: 500 },
      { text: "christopher@portfolio:~$ go build main.go", delay: 1000 },
      { text: "Compiling Go application...", delay: 1200 },
      { text: "✔ Build successful", delay: 500 },
      { text: "christopher@portfolio:~$ ./main", delay: 800 },
      { text: "Starting portfolio server on port 8080...", delay: 1000 },
      { text: "✔ Portfolio is running!", delay: 0, prompt: true }
    ];
  
    let currentCommand = 0;
    
    function typeNextCommand() {
      if(currentCommand >= commands.length) return;
      
      const command = commands[currentCommand];
      const line = document.createElement('div');
      line.className = 'terminal-line';
      
      if(command.prompt) {
        line.innerHTML = `<span class="terminal-prompt">${command.text}</span><span class="terminal-cursor"></span>`;
      } else {
        line.textContent = command.text;
      }
      
      terminal.appendChild(line);
      terminal.scrollTop = terminal.scrollHeight;
      
      currentCommand++;
      setTimeout(typeNextCommand, command.delay);
    }
  
    // Start typing after 1 second
    setTimeout(typeNextCommand, 1000);
  });