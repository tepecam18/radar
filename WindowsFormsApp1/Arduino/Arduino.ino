#include <Servo.h>
 
// Ultrasonik Sinyal pinleri
const int trigPin = 10;
const int echoPin = 11;
 
long duration;
int distance;
char onay;
 
Servo myServo; 
 
void setup() 
{
  pinMode(trigPin, OUTPUT); 
  pinMode(echoPin, INPUT); 
  Serial.begin(115200);
  myServo.attach(13); // Servo motor sinyal pini
}
void loop()
{
  if(Serial.available())
  {
    onay = Serial.read();
    if(onay == 'O')
    {
      Serial.println("OKEY");
        while(onay != 'Q')
        {
          for(int i=10;i<=170 and onay != 'Q';i++)
          { 
            onay = Serial.read();
            myServo.write(i); 
            delay(10); 
            distance = calculateDistance();
            Serial.print(i);
            Serial.print(",");
            Serial.print(distance);
            Serial.print(".");
            } 
          for(int i=165;i>15 and onay != 'Q';i--){
            onay = Serial.read();
            myServo.write(i);
            delay(10);
            distance = calculateDistance();
            Serial.print(i);
            Serial.print(",");
            Serial.print(distance);
            Serial.print(".");
          }
        }
    }
  }
}
 
int calculateDistance(){ 
   
  digitalWrite(trigPin, LOW); 
  delayMicroseconds(2);
 
  digitalWrite(trigPin, HIGH); 
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);
  duration = pulseIn(echoPin, HIGH); 
  distance= duration*0.034/2;
  if(distance > 50)
  {
    distance = 50; 
  }
  return distance;
}
