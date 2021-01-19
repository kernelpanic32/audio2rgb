# audio2rgb
This program takes the sound from your PC using the basswasapi.dll lib creates an audio spectrum and converts it into rgb and is sent to the arduino through serial, makeing them audiorythmic.

INSTALLATION

- Compile the solution with Visual Studio or download the exe from the releases

- Open audio2rgb.ino and upload to your arduino and connect your rgb leds

You would need to make a transistor driver if you are using a 12v led strip (arduino only provides 5v, 3.3v up to 500mah) google it.

       RED    -> pin 6
       GREEN  -> pin 3
       BLUE   -> pin 5

Then simply launch the program with your arduino connected via usb, click on connect, select your audio interface, click on enable and enojy!

Also when the arduino is not recieving any data it loops rgb colors.

If you can't compile this version you could try the legacy one (https://github.com/kernelpanic32/audio2rgb_legacy)
