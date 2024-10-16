let flowers = []; // Array to store multiple flowers
let rows = 60, cols = 120;

function setup() {
  createCanvas(700, 700, WEBGL);
  colorMode(HSB, 360, 100, 100);
  angleMode(DEGREES);
  noStroke();

  // Create multiple flowers and push them into the flowers array
  for (let i = 0; i < 1; i++) { 
    let flower = createFlower();
    flowers.push(flower);
  }
}

function draw() 
{
  // Set background color here
  background('#ffffff3d'); 

  orbitControl();

  rotateX(60);

  // Draw each flower from the flowers array
  for (let i = 0; i < flowers.length; i++) {
    drawFlower(flowers[i]);
  }
}

function createFlower() 
{
  let flower = {
    petals: [],
    pNum: random(3, 8), 
    fD: random(150, 250), 
    pLen: random(30, 80), 
    pSharp: random(0.2, 0.6), 
    fHeight: random(200, 400), 
    curve1: random(0.5, 1.0), 
    curve2: random(0.1, 0.5), 
    b: random(1.0, 3.0),
    bNum: random(5, 15) 
  };

  // Generate petals for the flower
  for (let theta = 0; theta < rows; theta++) 
    {
    flower.petals.push([]);
    for (let phi = 0; phi < cols; phi++) 
      {
      let r = (flower.pLen * pow(abs(sin(flower.pNum / 2 * phi * 360 / cols)), flower.pSharp) + flower.fD) * theta / rows;
      let x = r * cos(phi * 360 / cols);
      let y = r * sin(phi * 360 / cols);
      let z = vShape(flower.fHeight, r / 100, flower.curve1, flower.curve2, 1.5) - 200 +
        bumpiness(flower.b, r / 100, flower.bNum, phi * 360 / cols);

      let pos = createVector(x, y, z);
      flower.petals[theta].push(pos);
    }
  }

  return flower;
}

function drawFlower(flower)
 {
  for (let theta = 0; theta < flower.petals.length; theta++) 
    {
    fill(340, 100 - theta, 100);
    for (let phi = 0; phi < flower.petals[theta].length; phi++) 
      {
      if (theta < flower.petals.length - 1 && phi < flower.petals[theta].length - 1) 
        {
        beginShape();
        vertex(flower.petals[theta][phi].x, flower.petals[theta][phi].y, flower.petals[theta][phi].z);
        vertex(flower.petals[theta + 1][phi].x, flower.petals[theta + 1][phi].y, flower.petals[theta + 1][phi].z);
        vertex(flower.petals[theta + 1][phi + 1].x, flower.petals[theta + 1][phi + 1].y, flower.petals[theta + 1][phi + 1].z);
        vertex(flower.petals[theta][phi + 1].x, flower.petals[theta][phi + 1].y, flower.petals[theta][phi + 1].z);
        endShape(CLOSE);
      } 
      else if (theta < flower.petals.length - 1 && phi === flower.petals[theta].length - 1) 
        {
        beginShape();
        vertex(flower.petals[theta][phi].x, flower.petals[theta][phi].y, flower.petals[theta][phi].z);
        vertex(flower.petals[theta][0].x, flower.petals[theta][0].y, flower.petals[theta][0].z);
        vertex(flower.petals[theta + 1][0].x, flower.petals[theta + 1][0].y, flower.petals[theta + 1][0].z);
        vertex(flower.petals[theta + 1][phi].x, flower.petals[theta + 1][phi].y, flower.petals[theta + 1][phi].z);
        endShape(CLOSE);
      }
    }
  }
}

function vShape(A, r, a, b, c) 
{
  return A * pow(Math.E, -b * pow(abs(r), c)) * pow(abs(r), a);
}

function bumpiness(A, r, f, angle)
{
  return 1 + A * pow(r, 2) * sin(f * angle);
}
