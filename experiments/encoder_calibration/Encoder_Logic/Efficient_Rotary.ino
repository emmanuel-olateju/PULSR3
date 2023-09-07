#define pinA 2
#define pinB 3


float count = 0.0;
float angle = 0.0;
bool lastStateA;
bool newStateA;
String angle_str = " ";
String dataLabel1 = "Direction";
String dataLabel2 = "Angle";
String dir_A = "CW";
String dir_B = "CCW";
int freq = 1000;
bool label = true;

void setup() {
  // put your setup code here, to run once:

  Serial.begin(9600);
  pinMode(pinA,INPUT_PULLUP);
  pinMode(pinB,INPUT_PULLUP);


  attachInterrupt(digitalPinToInterrupt(pinA),interruptA,RISING);

  lastStateA = digitalRead(pinA);

}

void loop() {
  // put your main code here, to run repeatedly:
  // while(label){
  //   Serial.print(dataLabel1);
  //   Serial.print(",");
  //   Serial.println(dataLabel2);
  //   label = false;
  // }
  
  angle = (count / 600) * 360;
  Serial.print(angle_str);
  Serial.print(",");
  Serial.println(angle);
  delay(freq);
}

void interruptA()
{
  newStateA = digitalRead(pinA);
  
  if (newStateA != digitalRead(pinB))
  {
    if(count < 600){
    count++;
    angle_str = dir_A;
  }
    else if(count >= 600) {
      count = 0;
    }
  }

  else
  {
    if (count > -600){
    count--;
    angle_str = dir_B;
    }
    else if (count <= -600) {
      count = 0;
    }
  }
  
  lastStateA = newStateA;
}
