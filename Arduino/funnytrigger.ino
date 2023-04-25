/* www.circuits4you.com
   read a rotary encoder with interrupts
   Encoder hooked up with common to GROUND,    encoder0PinA to pin 2, encoder0PinB to pin 4
   it doesn't matter which encoder pin you use for A or B  
*/ 

#define encoder0PinA  2  //CLK Output A Do not use other pin for clock as we are using interrupt
#define encoder0PinB  4  //DT Output B
#define Switch 5 // Switch connection if available

volatile unsigned int encoder0Pos = 0;
int lastPinA;

void setup() { 

  pinMode(encoder0PinA, INPUT); 
  digitalWrite(encoder0PinA, HIGH);       // turn on pullup resistor
  pinMode(encoder0PinB, INPUT); 
  digitalWrite(encoder0PinB, HIGH);       // turn on pullup resistor
  attachInterrupt(digitalPinToInterrupt(encoder0PinA), doEncoder, CHANGE); // encoder pin on interrupt 0 - pin2
  lastPinA = digitalRead(encoder0PinA);
  Serial.begin (9600);
  Serial.println("start");                // a personal quirk
} 

void loop(){

}

void doEncoder() {
  int newPinA = digitalRead(encoder0PinA);
  if (newPinA != lastPinA)
  {
    lastPinA = newPinA;
    int BValue = digitalRead(encoder0PinB);
    if (newPinA == LOW && BValue == HIGH) {
      encoder0Pos++;
      Serial.print("Rotated clockwise ⏩, position: ");
      Serial.println (encoder0Pos); 
    }
    if (newPinA == LOW && BValue == LOW) {
      encoder0Pos--;
      Serial.print("Rotated counterclockwise ⏪, postition: ");
      Serial.println (encoder0Pos); 
    }
    
  }

}