import serial
import csv

arduino_port = "COM7"
baud_rate = 115200
fileName = "Rotary.csv"

ser = serial.Serial(arduino_port,baud_rate)
print("Connected to Arduino port:"+arduino_port)
file = open(fileName,"a")
print("Created file")

# Display the data to the terminal 


  

samples = 100
print_labels = False 
line = 0 
sensor_data = []

while line <= samples:
    getData = ser.readline()
    dataString = getData.decode('utf-8')
    data = dataString[0:][:-2]
    print(data)

    readings = data.split(",")
    print(readings)

    sensor_data.append(readings)
    print(sensor_data)

    line = line + 1


# adding arduino data to csv 
with open(fileName,'w',newline='') as f:
    writer = csv.writer(f)
    writer.writerows(sensor_data)

print("Data collection complete!")
file.close()
    