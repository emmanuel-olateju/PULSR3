const int HU_PIN = 2;
const int HV_PIN = 3;
const int HW_PIN = 4;


long timeU_0 = 0;
long timeU_1 = 0;
long timeD_0 = 0;
long timeD_1 = 0;

long interval_U = 0;
long interval_D = 0;

long nextChangeU = 0;
long nextChangeD = 0;

volatile unsigned long milliseconds = 0;

void setup() {
  // put your setup code here, to run once:
  pinMode(HU_PIN,INPUT);
  pinMode(HV_PIN,INPUT);
  pinMode(HW_PIN,OUTPUT);

  TCCR1A = 0;
  TCCR1B = (1 << WGM12) | (1 << CS11) | (1 << CS10); // CTC mode, prescaler 64
  OCR1A = 249; // Set the compare match value (16MHz / (64 * 1000 Hz) - 1)

  // Enable Timer1 compare match interrupt
  TIMSK1 = (1 << OCIE1A);

  attachInterrupt(digitalPinToInterrupt(HU_PIN),UInterrupt,CHANGE);
  attachInterrupt(digitalPinToInterrupt(HV_PIN),VInterrupt,CHANGE);

  sei();
}

void loop() {
  // put your main code here, to run repeatedly:
  long time_check = milliseconds;
  if (nextChangeU == time_check) {
    digitalWrite(HW_PIN,HIGH);
  }
  else if (nextChangeD == time_check) {
    digitalWrite(HW_PIN,LOW);
  }

}

ISR(TIMER1_COMPA_vect) {
  // Increment the milliseconds counter every 1 millisecond (1000 Hz)
  milliseconds++;
}

void UInterrupt() {
  if(digitalRead(HU_PIN)) {
    timeU_0 = milliseconds;
  }
  if(~digitalRead(HU_PIN)) {
    timeD_0 =milliseconds;
  }


}

void VInterrupt() {
  if(digitalRead(HV_PIN)) {
    timeU_1 = milliseconds;
    interval_U = timeU_1 - timeU_0;
    nextChangeU = timeU_1 + interval_U+5;

  }
  if(~digitalRead(HV_PIN)) {
    timeD_1 = milliseconds;
    interval_D = timeD_1 - timeD_0;
    nextChangeD = timeD_1 + interval_D+5;
  }

  

}

