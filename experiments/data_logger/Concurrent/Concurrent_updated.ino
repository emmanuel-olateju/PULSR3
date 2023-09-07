#include "conc.h"

void setup() {
  
  Serial.begin(115200);
  long init_time = millis();

  //pinmode declarations for encoders
  pinMode(encoderCLK_A,INPUT_PULLUP);
  pinMode(encoderDT_A,INPUT_PULLUP);
  pinMode(encoderCLK_B,INPUT_PULLUP);
  pinMode(encoderDT_B,INPUT_PULLUP);

  //interrupts for encoders 
  attachInterrupt(digitalPinToInterrupt(encoderCLK_A),interruptA,RISING);
  attachInterrupt(digitalPinToInterrupt(encoderCLK_B),interruptB,RISING);

  //variables to track the state of the encoders to know the angular rotation of encoders 
  lastStateA = digitalRead(encoderCLK_A);
  lastStateC = digitalRead(encoderCLK_B);

  // activationg each of the Loadcells
  Loadcell[0].begin(HX711_dout1,HX711_sck1);
  Loadcell[1].begin(HX711_dout2,HX711_sck2);
  Loadcell[2].begin(HX711_dout3,HX711_sck3);

}

void loop() {

  // code to start data collection prompt 
  while(start_new)
  {
    Serial.println("New Sample colletion. ");
    Serial.println("Press key 'b' to start data collection");

    // loop to ensure that the serial module is available 
    while (!Serial.available()){}

    // variable to keep track of input from the serial console 
    char inputChar = Serial.read();
    start_new = false;
    Serial.flush();
  }

  
  //condition to enable data collection based on angular orientation and the number of iterations of interest
  if ((inputChar == dataKey) & (iter < 12) & (((int)(angle1 * 100) % mod_factor) == 0)) 
  {
    collect_data();
    delay(freq);
  }


  else if(iter >= 12)
  {
    Serial.println("Data collection complete!!!!");
    Serial.print("Press key 'p' to start the data logging: ");
    inputChar = Serial.read();

    if(inputChar == 'p')
    {
      log_data();
    }
    
  }
}
