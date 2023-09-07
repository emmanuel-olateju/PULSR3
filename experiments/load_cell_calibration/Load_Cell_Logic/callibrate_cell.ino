
#include "cell_load.h"



void setup() {
  Serial.begin(115200); 
  delay(10);
  Serial.println();
  Serial.println("Starting...");

  LoadCell.begin();
  //LoadCell.setReverseOutput(); //uncomment to turn a negative output value to positive
  unsigned long stabilizingtime = 2000; // preciscion right after power-up can be improved by adding a few seconds of stabilizing time
  boolean _tare = true; //set this to false if you don't want tare to be performed in the next step
  LoadCell.start(stabilizingtime, _tare);

  if (LoadCell.getTareTimeoutFlag() || LoadCell.getSignalTimeoutFlag()) 
  {
    Serial.println("Timeout, check MCU>HX711 wiring and pin designations");
    while (1);
  }

  else {
    LoadCell.setCalFactor(1.0); // user set calibration value (float), initial value 1.0 may be used for this sketch
    Serial.println("Startup is complete");
  }
  while (!LoadCell.update());
  calibrate(); //start calibration procedure
}

void loop() {
  // static boolean newDataReady = 0;
  // const int serialPrintInterval = 0; //increase value to slow down serial print activity

  // check for new data/start next conversion:
  // if (LoadCell.update()) newDataReady = true;

  // // get smoothed value from the dataset:
  // if (newDataReady) {
  //   if (millis() > t + serialPrintInterval) {
  //     float i = LoadCell.getData();
  //     Serial.print("Load_cell output val: ");
  //     Serial.println(i);
  //     newDataReady = 0;
  //     t = millis();
  //   }
  // }

  // receive command from serial terminal
  if (Serial.available() > 0) {
    char inByte = Serial.read();
    if (inByte == 't') LoadCell.tareNoDelay(); //tare
    else if (inByte == 'r') calibrate(); //calibrate
    // else if (inByte == 'c') changeSavedCalFactor(); //edit calibration value manually
  }

  // check if last tare operation is complete
  if (LoadCell.getTareStatus() == true) {
    Serial.println("Tare complete");
  }

}



  