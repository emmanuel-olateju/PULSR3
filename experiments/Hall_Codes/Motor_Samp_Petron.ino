const int HU_PIN = 2;
const int HV_PIN = 3;
const int HW_PIN = 4;

volatile unsigned long milliseconds = 0;
// volatile bool HU_FLAG = 0;
// volatile bool HV_FLAG = 0;
// volatile bool HW_FLAG = 0;
unsigned long oneSixthOfCycle = 0;
unsigned long T0=0;
unsigned long T1=0;

bool startup = 1;

void setup() {
  Serial.begin(9600);
  pinMode(HU_PIN,INPUT);
  pinMode(HV_PIN,INPUT);
  pinMode(HW_PIN,OUTPUT);

  TCCR1A = 0;
  TCCR1B = (1 << WGM12) | (1 << CS11) | (1 << CS10); // CTC mode, prescaler 64
  OCR1A = 249; // Set the compare match value (16MHz / (64 * 1000 Hz) - 1)

  // Enable Timer1 compare match interrupt
  TIMSK1 = (1 << OCIE1A);

  attachInterrupt(digitalPinToInterrupt(HU_PIN),uPinInterrupt,CHANGE);
  attachInterrupt(digitalPinToInterrupt(HV_PIN),vPinInterrupt,CHANGE);
  sei();
}

void loop() {
  // put your main code here, to run repeatedly:
  if(startup==0){
    T1=milliseconds;
    if((T1-T0)>=oneSixthOfCycle){
      if(digitalRead(HV_PIN))
        digitalWrite(HW_PIN,0);
      else if(~digitalRead(HV_PIN))
        digitalWrite(HW_PIN,1);
    }
  }
  // Serial.print(digitalRead(HU_PIN));
  // Serial.println(digitalRead(HV_PIN));
  // Serial.print(HW_PIN);
  // Serial.println('\n');
}

ISR(TIMER1_COMPA_vect) {
  // Increment the milliseconds counter every 1 millisecond (1000 Hz)
  milliseconds++;
}

void uPinInterrupt(){
  // Serial.print("u");
  // Serial.print(' ');
  if(startup==1)T0=milliseconds;
}
void vPinInterrupt(){
  // Serial.println("v");
  if(startup==1){
    T1=milliseconds; 
    oneSixthOfCycle = T1-T0;
    startup=0;
  }
  T0 = milliseconds;
}