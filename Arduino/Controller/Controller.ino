// variables for LED test
int incomingByte[2];
int ledPin = 11;
bool LED = false;

// rotary encoder stuff
int counter = 0;
bool direction;
int pin_clk_last;
int pin_clk_current;
int pin_clk = 3;
int pin_dt = 4;
int button_pin = 5;

// light barrier stuff
int lightSensor = 9;
int lightvalue;
int lastlightvalue;

void setup() {
  // led test
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);

  // rotary encoder test
  pinMode(pin_clk,INPUT);
  pinMode(pin_dt,INPUT);
  pinMode(button_pin,INPUT);
  digitalWrite(pin_clk,true);
  digitalWrite(pin_dt,true);
  digitalWrite(button_pin,true);
  pin_clk_last = digitalRead(pin_clk);
  counter = 0;

  // light barrier test
  pinMode (lightSensor, INPUT);
  digitalWrite(lightSensor, HIGH);

  Serial.begin(9600);
}

void loop() {
  // led test stuff
  if (Serial.available() > 0){
    while (Serial.peek() == 'L'){
      Serial.read();
      incomingByte[0] = Serial.parseInt();
      if (incomingByte[0] == 1) {LED = true;} else {LED = false;}
    }
    while (Serial.available() > 0){
      Serial.read();
    }
  }
  ledCheck();

  // rotary encoder check
  pin_clk_current = digitalRead(pin_clk);
  if (pin_clk_current != pin_clk_last){
    if (digitalRead(pin_dt) != pin_clk_current){
      // pin clk changed
      counter ++;
      direction = true;
    }
    else {
      // pin_dt changed
      counter --;
      direction = false;
    }
    Serial.println(counter);
  }
  pin_clk_last = pin_clk_current;
  if (!digitalRead(button_pin) && counter != 0){
    counter = 0;
    Serial.println("reset position");
  }

  // light barrier test:
  lightvalue = digitalRead(lightSensor);
  if (lightvalue == HIGH)
  {
    if (lastlightvalue != lightvalue)
    {
      Serial.println("Light Barrier Closed");
    }
  }
  else
  {
    if (lastlightvalue != lightvalue)
    {
      Serial.println("Light Barrier Open");
    }
  }
  lastlightvalue = lightvalue;
}

void ledCheck()
{
  if (LED){
    digitalWrite(ledPin, HIGH);
  } else {
    digitalWrite(ledPin, LOW);
  }
  return;
}
