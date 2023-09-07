


#include <HX711_ADC.h>
//#include <HX711.h>

//pins:
const int HX711_dout = 6; //mcu > HX711 dout pin
const int HX711_sck = 5; //mcu > HX711 sck pin

//HX711 constructor:
HX711_ADC LoadCell(HX711_dout, HX711_sck);


unsigned long t = 0;

void setup() {
  Serial.begin(115200); delay(10);
  // Serial.println();
  // Serial.println("Starting...");

  LoadCell.begin();

  
  float calibrationValue; // calibration value (see example file "Calibration.ino")
  calibrationValue = 22.86; //enter the calibration value gotten from the callibration code


  unsigned long stabilizingtime = 2000; // preciscion right after power-up can be improved by adding a few seconds of stabilizing time
  boolean _tare = true; //set this to false if you don't want tare to be performed in the next step
  LoadCell.start(stabilizingtime, _tare);

  if (LoadCell.getTareTimeoutFlag()) {
    Serial.println("Timeout, check MCU>HX711 wiring and pin designations");
    while (1);
  }
  else {
    LoadCell.setCalFactor(calibrationValue); // set calibration value (float)
    // Serial.println("Startup is complete");
  }
  delay(1000);
}

void loop() {
  static boolean newDataReady = 0;
  const int serialPrintInterval = 0; //increase value to slow down serial print activity

  // check for new data/start next conversion:
  if (LoadCell.update()) newDataReady = true;

  // get smoothed value from the dataset:
  if (newDataReady) {
    if (millis() > t + serialPrintInterval) {
       float i = LoadCell.getData();
      //float i = scale.read_medavg(10);
      // Serial.print("Load_cell output val: ");
      Serial.println(i);
      newDataReady = 0;
      t = millis();
      delay(200);
    }
  }

}