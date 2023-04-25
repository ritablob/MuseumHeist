/* www.circuits4you.com
   read a rotary encoder with interrupts
   Encoder hooked up with common to GROUND,    encoder0PinA to pin 2, encoder0PinB to pin 4
   it doesn't matter which encoder pin you use for A or B  
*/ 
#include "pitches.h"

#define encoder0PinA  2  //CLK Output A Do not use other pin for clock as we are using interrupt
#define encoder0PinB  4  //DT Output B
#define Switch 5 // Switch connection if available
#define lightBarrier 10
#define passiveBuzzer 9


volatile unsigned int encoder0Pos = 0;
int lastPinA;
int lightValue;
bool sound;
int melody[] = {

  NOTE_C4, NOTE_G3, NOTE_G3, NOTE_A3, NOTE_G3, 0, NOTE_B3, NOTE_C4
};

// note durations: 4 = quarter note, 8 = eighth note, etc.:
int noteDurations[] = {

  4, 8, 8, 4, 4, 4, 4, 4
};

void setup() { 

pinMode (passiveBuzzer, OUTPUT);
  pinMode (lightBarrier, INPUT); // Initialize sensor pin
  digitalWrite(lightBarrier, HIGH);
  pinMode(encoder0PinA, INPUT); 
  digitalWrite(encoder0PinA, HIGH);       // turn on pullup resistor
  pinMode(encoder0PinB, INPUT); 
  digitalWrite(encoder0PinB, HIGH);       // turn on pullup resistor
  attachInterrupt(digitalPinToInterrupt(encoder0PinA), doEncoder, CHANGE); // encoder pin on interrupt 0 - pin2
  lastPinA = digitalRead(encoder0PinA);
  sound = true;
  Serial.begin (9600);
  Serial.println("start");   
  Sound();
     // a personal quirk
} 

void loop(){
  lightValue = digitalRead(lightBarrier);
  Serial.print("light value - ");
  Serial.println(lightValue);
  if (lightValue == 1)
  {
    Sound();
  }
}

void Sound(){
  // iterate over the notes of the melody:

  for (int thisNote = 0; thisNote < 8; thisNote++) {

    // to calculate the note duration, take one second divided by the note type.

    //e.g. quarter note = 1000 / 4, eighth note = 1000/8, etc.

    int noteDuration = 1000 / noteDurations[thisNote];

    tone(passiveBuzzer, melody[thisNote], noteDuration);

    // to distinguish the notes, set a minimum time between them.

    // the note's duration + 30% seems to work well:

    int pauseBetweenNotes = noteDuration * 1.30;

    delay(pauseBetweenNotes);

    // stop the tone playing:

    noTone(8);
  }   
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