document.addEventListener('DOMContentLoaded', function() {
    const skills = [
      "Java", "Python", "C++", "JavaScript", 
      "HTML5", "CSS3", "React", "Node.js",
      "SQL", "Git", "AWS", "Docker", "Algorithms",
      "Data Structures", "Spring Boot", "REST APIs"
    ];
  
    const container = document.getElementById('skill-cloud');
    const width = container.offsetWidth;
    const height = 500; // Increased height
  
    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000);
    const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
    
    renderer.setSize(width, height);
    container.appendChild(renderer.domElement);
  
    // Create larger skill tags
    const tags = [];
    const textureLoader = new THREE.TextureLoader();
    
    skills.forEach((skill, i) => {
      const canvas = document.createElement('canvas');
      const context = canvas.getContext('2d');
      canvas.width = 512; // Increased size
      canvas.height = 256; // Increased size
      
      // Improved text rendering
      context.fillStyle = 'rgba(0, 47, 92, 0.9)';
      context.fillRect(0, 0, canvas.width, canvas.height);
      context.font = 'Bold 42px Montserrat'; // Larger font
      context.textAlign = 'center';
      context.fillStyle = 'white';
      context.fillText(skill, canvas.width/2, canvas.height/2 + 15);
      
      const texture = new THREE.CanvasTexture(canvas);
      const material = new THREE.SpriteMaterial({ map: texture });
      const sprite = new THREE.Sprite(material);
      
      // Position in a larger sphere
      const radius = 15;
      const theta = Math.random() * Math.PI * 2;
      const phi = Math.random() * Math.PI;
      
      sprite.position.x = radius * Math.sin(phi) * Math.cos(theta);
      sprite.position.y = radius * Math.sin(phi) * Math.sin(theta);
      sprite.position.z = radius * Math.cos(phi);
      
      sprite.scale.set(4, 2, 1); // Larger scale
      
      scene.add(sprite);
      tags.push({
        sprite: sprite,
        velocity: new THREE.Vector3(
          (Math.random() - 0.5) * 0.02,
          (Math.random() - 0.5) * 0.02,
          (Math.random() - 0.5) * 0.02
        )
      });
    });
  
    camera.position.z = 25; // Adjusted for larger view
    const controls = new THREE.OrbitControls(camera, renderer.domElement);
    controls.enableZoom = true;
    controls.autoRotate = true;
    controls.autoRotateSpeed = 0.5;
  
    // Add subtle floating animation
    function animate() {
      requestAnimationFrame(animate);
      
      tags.forEach(tag => {
        tag.sprite.position.add(tag.velocity);
        
        // Boundary check
        if (tag.sprite.position.length() > 20) {
          tag.velocity.multiplyScalar(-0.5);
        }
      });
      
      controls.update();
      renderer.render(scene, camera);
    }
    
    window.addEventListener('resize', function() {
      const newWidth = container.offsetWidth;
      camera.aspect = newWidth / height;
      camera.updateProjectionMatrix();
      renderer.setSize(newWidth, height);
    });
  
    animate();
  });