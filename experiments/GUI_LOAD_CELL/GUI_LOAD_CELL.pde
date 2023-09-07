
import meter.*;
import processing.serial.*;

Serial port;
String list;

Meter m;

float meterValue = 0;

void setup() {
  size(950, 400);
  background(0);
  
  port = new Serial(this, "COM7", 115200);

  fill(120, 50, 0);
  m = new Meter(this, 25, 100);
  // Adjust font color of meter value  
  m.setTitleFontSize(20);
  m.setTitleFontName("Arial bold");
  m.setTitle("Force(N)");
  m.setDisplayDigitalMeterValue(true);
  
  // Meter Scale
  String[] scaleLabelsT = {"0", "50", "100", "150", "200", "250", "300"};
  m.setScaleLabels(scaleLabelsT);
  m.setScaleFontSize(18);
  m.setScaleFontName("Times New Roman bold");
  m.setScaleFontColor(color(200, 30, 70));
  
  m.setArcColor(color(141, 113, 178));
  m.setArcThickness(10);
  m.setMaxScaleValue(80);
  
  m.setNeedleThickness(3);
  
  m.setMinInputSignal(0);
  m.setMaxInputSignal(300);  
}

public void draw() {
  background(0);
  textSize(30);
  fill(0, 255, 0);
  PFont f = createFont("Georgia",70);
  textFont(f);
  textSize(32);
  if (port.available() > 0) {
     list = port.readString();
    
    float force = float(list);
    
    
    println("Force: " + force + " N  ");
    
    if ( force > 0)
      m.updateMeter(int(force));
     else
     {
       int k = 0;
       m.updateMeter(k);
     }
    text("Force(N): "+ force ,10,64); 
  }
  
}


void keyPressed() {
  if (key == 'q' || key == 'Q') {
    exit();  // Quit the sketch when 'q' or 'Q' is pressed
  }
}
