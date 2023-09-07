#ifndef CONC_H
#define CONC_H
#define encoderCLK_A 21
#define encoderCLK_B 22
#define encoderDT_A 23
#define encoderDT_B 24
#define HX711_dout1 25
#define HX711_sck1 26
#define HX711_dout2 27
#define HX711_sck2 32
#define HX711_dout3 33
#define HX711_sck3 19
#include <HX711.h>
#endif

HX711 Loadcell[3];
String dataLabel[6] = {"Encoder1","Encoder2","Arm_Load1","Arm_Load2","S_Cell","Current_Time"};

long  result[3];
float arrptr[3];
unsigned long t = 0;


void interruptA();
void interruptB();

void cell_available();
void collect_data();
void log_data();

bool control = true;
bool start_new = true;
const byte dataKey = 'b';

int iter = 0 ;
int count1 = 0;
int samples = 50;

float angle1 = 0.0;
float angle2 = 0.0;
long curr_time = 0.0;
long init_time = 0.0;
int count2 = 0;


bool lastStateA;
bool newStateA;

bool lastStateC;
bool newStateC;

int freq = 1000;
int mod_factor = 3000;


int results[12][samples][6] = {0}; //array to collect data 


int adder = 0;

bool label = true;

// interrrupt for rotatry encoderA
void interruptA()
{
  newStateA = digitalRead(encoderCLK_A);
  
  if (newStateA != digitalRead(encoderDT_A))
  {
    if(count1 < 600){
    count1++;
  }
    else if(count1 >= 600) {
      count1 = 0;
    }
  }

  else
  {
    if (count1 > -600){
    count1--;
    
    }
    else if (count1 <= -600) {
      count1 = 0;
    }
  }
  
  lastStateA = newStateA;
}

// interrupt for rotary encoderB
void interruptB()
{
  newStateC = digitalRead(encoderCLK_B);
  
  if (newStateC != digitalRead(encoderDT_B))
  {
    if(count2 < 600){
    count2++;
   
  }
    else if(count2 >= 600) {
      count2 = 0;
    }
  }

  else
  {
    if (count2 > -600){
    count2--;
    
    }
    else if (count2 <= -600) {
      count2 = 0;
    }
  }
  
  lastStateC = newStateC;
}

// reading data from the load cells and returning the values
void cell_available()
{
  static boolean newDataReady[3] = {0};
  const int serialPrintInterval = 0;

  for(int i = 0; i <3 ;i++)
  {
    if(Loadcell[i].is_ready()) newDataReady[i] = true;
  }

  for(int i = 0; i<3 ; i++)
  {
    if(newDataReady[i])
    {
      if (millis() > t + serialPrintInterval)
       {
      result[i] = Loadcell[i].read();
      newDataReady[i] = 0;
      t = millis();
      delay(200);
      }
    }

  }
  for (int i =0;i<3;i++) arrptr[i] = result[i];
}

// function to collect_data from the sensors and storing them into an array
void collect_data()
{
  curr_time = millis();

  if (iter < 12) control = true;
  
 
 while (control)
 {
   
  for (size_t j = 0; j < samples; j++)
    {
      cell_available(); 
      angle1 = abs((count1/600)*360);
      angle2 = abs((count2/600)*360);
      int compute_time = curr_time - init_time;

      for (size_t k = 0; k < 6; k++)
      { 
        if ( k == 0) results[iter][j][k] = angle1;
        if ( k == 1) results[iter][j][k] = angle2;
        if ( k == 2) results[iter][j][k] = compute_time;
        if ( k == 3) results[iter][j][k] = arrptr[0];
        if ( k == 4) results[iter][j][k] = arrptr[1];
        if ( k == 5) results[iter][j][k] = arrptr[2];
      }
      
    }

    iter++;
    control  = false;
  }
}

// function to serial print data
void log_data()
{
  Serial.println("Data Logging process initiated!!");
  Serial.flush();
  while(label)
    {
      for (size_t i = 0; i < 6; i++)
      {
        Serial.print(dataLabel[i]);
        Serial.print(",");

        if(i == 5) {
          Serial.println(dataLabel[i]);
          label = false;
        }
      }
    }

  for (size_t i = 0;  i < 12; i++)
    {
      for (size_t j = 0; j <samples; j++)
      { 
        for (size_t k = 0; k < 6; k++)
        {
          Serial.print(results[i][j][k]);
          Serial.print(',');
          if ( k == 5) Serial.println(results[i][j][k]);
        }
        
      } 
    }
    iter = 0;
    Serial.println("Data Logging Complete!!!");
    Serial.flush();
}


