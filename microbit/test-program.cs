// first pass attempt to control the display on the microbit using REPL on the serial interface
// see https://microbit-micropython.readthedocs.io/en/v1.1.1/devguide/repl.html
//
using System;  
using System.IO.Ports;  
  
class Program  
{  
    static void Main(string[] args)  
    {  
        // Connect to the micro:bit serial port  
        var portName = "COM3"; // Replace with your micro:bit's serial port name  
        var serialPort = new SerialPort(portName, 115200);  
        serialPort.Open();  
  
        // Clear any existing data in the serial input buffer  
        serialPort.DiscardInBuffer();  
  
        // Send commands to the micro:bit 
        SendCommand(serialPort, "display.clear()");   
        SendCommand(serialPort, "display.set_pixel(0,0,5)");  
        SendCommand(serialPort, "display.set_pixel(0,4,5)");  
        //SendCommand(serialPort, "display.show('score')");  
        // Close the serial port connection 
        Console.ReadLine(); 
        serialPort.Close();  
    }  
  
    static void SendCommand(SerialPort serialPort, string command)  
    {  
        //send crt-c to stop any running program
        serialPort.Write(new byte[] { 0x03 }, 0, 1);
        // Wait for the micro:bit to stop any running program
        System.Threading.Thread.Sleep(1000); // Adjust the delay as needed

        // Send the command to the micro:bit  
        serialPort.WriteLine(command); 
        // send a carriage return to execute the command 
        serialPort.Write(new byte[] { 0x0d }, 0, 1);
        // Wait for the micro:bit to finish executing the command  
        System.Threading.Thread.Sleep(1000); // Adjust the delay as needed  
  
        // Read the response from the micro:bit  
        var response = serialPort.ReadExisting();  
  
        // Print the response to the console  
        Console.WriteLine(response);  
    }  
}  
