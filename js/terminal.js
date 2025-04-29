document.addEventListener('DOMContentLoaded', function() {
    const terminal = document.getElementById('terminal');
    const commands = [
      { text: "npm init portfolio", delay: 800 },
      { text: "Installing dependencies...", delay: 1000 },
      { text: "✔ React", delay: 200 },
      { text: "✔ Node.js", delay: 200 },
      { text: "✔ Java", delay: 200 },
      { text: "✔ Python", delay: 200 },
      { text: "Building project...", delay: 1200 },
      { text: "Optimizing performance...", delay: 1000 },
      { text: "Deploying to AWS...", delay: 800 },
      { text: "✔ Deployment successful!", delay: 500 },
      { text: "christopher@portfolio:~$ ", delay: 0, prompt: true }
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