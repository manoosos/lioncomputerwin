LionComputerWin
-------
(or **LionWin** for short)

A Windows emulator for the LionComputer, in C#.

Code design: **Manos Kantzos**

Essential Contribution: **Theodoulos Liontakis, Ian Smirlis** (thank you all!)

The **LionComputer** is an FPGA based CPU-Computer, designed By Theodoulos Liontakis.

LionComputer Project Page:

http://users.sch.gr/tliontakis/index.php/my-projects/13-vhdl-cpu

LionComputer Assembler:

https://github.com/lliont/Lionasm

LionWin uses the NAudio library:

https://github.com/naudio/NAudio

Before the first solution run, make sure to 

- Uncheck, if not unchecked already, the DEBUG compiler constant for the two projects (the LionComputerEmulator class library and the LionWin frontend).

- Set the LionWin frontend project, as StartUp Project.


To run the LionComputer in one click:

- Choose **File->Open BIN** and select the **system.bin**. This will load a snapshot of the latest system of the computer. 

- Press the **[RUN]** button. The system will run the Lion Basic and wait for commands.


To normally run the LionComputer:

- First you should **File->Load BIN** the **lionrom.asm.bin**.

- Then you should **File->Load BIN at Address** the **lionbasic.asm.bin.** Load it at the hex address **$2436**.

- Press **[RUN]**. The system will run the Lion Basic and wait for commands.


Then you could load the demo game [**'Astro'**](https://www.youtube.com/watch?v=rEBGKEbPayw):

- **File->Load BIN at Address** and choose the file type **'RBN'** (relative BIN). Load the **astro.asm.rbn** at the decimal address **20000**.

- After the load to memory and in the Lion Basic, type **RCODE20000** to run the **Astro** game code.

- Use the Windows Keyboard Arrows as a joystick, the Control key as a fire button and enjoy the game!


The LionWin emulator does not emulate the hardware SPI and the FAT file system at the moment, so the Lion Basic commands LOAD, SAVE, DIR, DELETE, LCODE, SCODE do nothing. The file i/o is done at the frontend though, at the 'File' menu.


This is not a complete manual and more updates should be done!
