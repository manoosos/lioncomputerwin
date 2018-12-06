; .Lion
heap_ptr equ 46000
ram_start equ 46002
strMsg EQU 46004   ; Ljava/lang/String;
sp1_data EQU 46006   ; [I
dot_data EQU 46008   ; [I
rock_data EQU 46010   ; [I
RAND EQU 46012   ; I
buffer EQU 46014   ; I
bt EQU 46016   ; I
i EQU 46018   ; I
hit EQU 46020   ; I
level EQU 46022   ; I
score EQU 46024   ; I
hiscore EQU 46026   ; I
sx EQU 46028   ; F
sy EQU 46032   ; F
sdx EQU 46036   ; F
sdy EQU 46040   ; F
distx EQU 46044   ; F
disty EQU 46048   ; F
fdistx EQU 46052   ; F
fdisty EQU 46056   ; F
bx EQU 46060   ; F
by EQU 46064   ; F
bdx EQU 46068   ; F
bdy EQU 46072   ; F
brange EQU 46076   ; F
rockx EQU 46080   ; [F
rocky EQU 46082   ; [F
rockdx EQU 46084   ; [F
rockdy EQU 46086   ; [F
rockon EQU 46088   ; [I
 SDCBUF1	EQU	$2000  ;DS	514  Buffer 1 
 SDCBUF2	EQU	$2202  ;DS	514  Buffer 2 
 FATBOOT	EQU	$2404  ;DS	2    Fat boot #sector 
 FATROOT	EQU	$2406  ;DS	2    Root directory #sector 
 FSTCLST	EQU	$2408  ;DS	2    First data #sector 
 FSTFAT	EQU	$240a  ;DS	2    First Fat first #sector 
 SDFLAG	EQU	$240c  ;DS	2    SD card initialized by rom=256 
 COUNTER	EQU	$240e  ;DS	2    General use counter increased by int 0  
 FRAC1     EQU	$2410  ;DS	2    for fixed point multiplication-division 
 FRAC2     EQU $2412  ;DS	2               >> 
 RHINT0	EQU	$2414  ; Hardware interrupt 0 
 RHINT1	EQU	$2418  ; Hardware interrupt 1 
 RHINT2	EQU	$241c 
 RINT6 	EQU	$2420 
 RINT7 	EQU	$2424 
 RINT8 	EQU	$2428 
 RINT9 	EQU	$242c 
 RINT15	EQU	$2430 
ORG     	0  

start:
  ;; Set up heap and static initializers
  MOV  (heap_ptr),46090
  GADR A7, _strMsg
  MOV (strMsg), A7
  GADR A7, _sp1_data
  MOV (sp1_data), A7
  GADR A7, _dot_data
  MOV (dot_data), A7
  GADR A7, _rock_data
  MOV (rock_data), A7
  MOV (RAND), 197
  MOV (buffer), 2
  MOV (bt), 0
  GADR A7, _rockx
  MOV (rockx), A7
  GADR A7, _rocky
  MOV (rocky), A7
  GADR A7, _rockdx
  MOV (rockdx), A7
  GADR A7, _rockdy
  MOV (rockdy), A7
  GADR A7, _rockon
  MOV (rockon), A7

main:
  PUSH A1
  PUSH A2
  PUSH A3
  PUSH A4
  PUSH A5
  PUSH A6
  PUSH A7
  PUSHX
  MOV A3,SP
  SUB SP, 28
  MOV A6,SP
  ADDI A6,2
  ; set_integer_local(0,0)
  MOV	A3,A6
  ADDI A3,0
  MOV (A3), $0000
  ; set_integer_local(6,0)
  MOV	A3,A6
  ADDI A3,12
  MOV (A3), $0000
  ; push_int(1)
  PUSH $0001
  ; put static
  POP A7
  MOV (hiscore), A7
main_9:
  ; push_local_int(6)
  MOV	A3,A6
  ADDI A3,12
  PUSH (A3)
  ; push_int(81)
  PUSH $0051
  ;; jump_cond_integer(main_2020,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_2020
  ; push_local_int(6)
  MOV	A3,A6
  ADDI A3,12
  PUSH (A3)
  ; push_int(113)
  PUSH $0071
  ;; jump_cond_integer(main_2020,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_2020
  ; push_int(1)
  PUSH $0001
  ; put static
  POP A7
  MOV (level), A7
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (score), A7
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (hit), A7
  ; cls
  MOVI A0,3
  INT 4
  ; push_int(4)
  PUSH $0004
  ; push_int(3)
  PUSH $0003
  ;screen
  POP A1
  POP A0
  SLL A1,3
  ADD A1,A0
  SETX 1983
  MOV A0,61152
  Label_0: OUT.B A0,A1
  INC A0
  JRX Label_0
  ; push_int(1)
  PUSH $0001
  ; push_int(10)
  PUSH $000a
  ; get static
  PUSH (strMsg)
  ;; putSTR
  POP A1
  POP A0
  POP A2
  MOVHL A2,A0
  MOVI A0,5
  INT 4
  ; push_int(1)
  PUSH $0001
  ; push_int(55)
  PUSH $0037
  ;; push_ref_static(144)
  GADR A7, _string_144
  PUSH A7
  ;; putSTR
  POP A1
  POP A0
  POP A2
  MOVHL A2,A0
  MOVI A0,5
  INT 4
  ; push_int(1)
  PUSH $0001
  ; push_int(1)
  PUSH $0001
  ;; push_ref_static(145)
  GADR A7, _string_145
  PUSH A7
  ;; putSTR
  POP A1
  POP A0
  POP A2
  MOVHL A2,A0
  MOVI A0,5
  INT 4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, brange
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; set_integer_local(1,0)
  MOV	A3,A6
  ADDI A3,2
  MOV (A3), $0000
main_73:
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_int(32)
  PUSH $0020
  ;; jump_cond_integer(main_153,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_153
  ; push_int(63552)
  PUSH $f840
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ;; push_ref(sp1_data)
  PUSH (sp1_data)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,1
  ADD A7, A4
  PUSH (A7)
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ; push_int(63584)
  PUSH $f860
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ;; push_ref(dot_data)
  PUSH (dot_data)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,1
  ADD A7, A4
  PUSH (A7)
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (i), A7
main_107:
  ;; push_ref(i)
  PUSH (i)
  ; push_int(8)
  PUSH $0008
  ;; jump_cond_integer(main_147,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_147
  ; push_int(63552)
  PUSH $f840
  ; push_int(2)
  PUSH $0002
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_int(32)
  PUSH $0020
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ;; push_ref(rock_data)
  PUSH (rock_data)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,1
  ADD A7, A4
  PUSH (A7)
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; put static
  POP A7
  MOV (i), A7
  JR main_107
main_147:
  ;; inc_integer(1,1)
  MOV	A3,A6
  ADDI A3,2
  ADD (A3),1
  JR main_73
main_153:
  ; push_int(0)
  PUSH $0000
  ; push_int(0)
  PUSH $0000
  ; push_int(1)
  PUSH $0001
  ; push_int(100)
  PUSH $0064
  ; push_int(60)
  PUSH $003c
  ; push_int(6)
  PUSH $0006
  ;; invoke_static_method(set_sprite_IIIIII,6,1)
  ADD SP,12
  MOV A7,SP
  SUB SP,18
  SETX 5
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, 8
  SETX 5
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,18
  JRSR set_sprite_IIIIII
  ; push_int(1)
  PUSH $0001
  ; push_int(0)
  PUSH $0000
  ; push_int(0)
  PUSH $0000
  ; push_int(100)
  PUSH $0064
  ; push_int(60)
  PUSH $003c
  ; push_int(6)
  PUSH $0006
  ;; invoke_static_method(set_sprite_IIIIII,6,1)
  ADD SP,12
  MOV A7,SP
  SUB SP,18
  SETX 5
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, 8
  SETX 5
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,18
  JRSR set_sprite_IIIIII
  ;; push_ref(level)
  PUSH (level)
  ;; invoke_static_method(init_I,1,1)
  ADD SP,2
  MOV A7,SP
  SUB SP,8
  SETX 0
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, -2
  SETX 0
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,8
  JRSR init_I
  ; timer
  IN	A1,20
  BCLR A1,15
  PUSH A1
  ;; pop_local_var_int(3)
  POP A7
  MOV A3,A6
  ADDI A3,6
  MOV (A3),A7
  ; push_int(20)
  PUSH $0014
  ; push_int(2)
  PUSH $0002
  ; out 
  POP A1
  POP A0
  OUT A0,A1
  ;push float 180.000000 = 17204 0
  PUSH $4334
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 118.000000 = 17132 0
  PUSH $42ec
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; set_integer_local(4,0)
  MOV	A3,A6
  ADDI A3,8
  MOV (A3), $0000
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (hit), A7
  ; set_integer_local(5,0)
  MOV	A3,A6
  ADDI A3,10
  MOV (A3), $0000
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (buffer), A7
  ; push_int(20)
  PUSH $0014
  ;; push_ref(buffer)
  PUSH (buffer)
  ; out 
  POP A1
  POP A0
  OUT A0,A1
  ; set_integer_local(7,0)
  MOV	A3,A6
  ADDI A3,14
  MOV (A3), $0000
  ; set_integer_local(10,0)
  MOV	A3,A6
  ADD A3,20
  MOV (A3), $0000
  ; push_int(11)
  PUSH $000b
  ; push_int(1)
  PUSH $0001
  ; out 
  POP A1
  POP A0
  OUT A0,A1
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (i), A7
main_249:
  ;; push_ref(i)
  PUSH (i)
  ; push_int(100)
  PUSH $0064
  ;; jump_cond_integer(main_284,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_284
  ; push_int(380)
  PUSH $017c
  ; rnd
  POP A1
  MOV A0,(RAND)
  MOV A2,997
  MULU A2,A0
  IN A0,20
  ADD A2,A0
  MOV (RAND),A2
  MOVI A0,6
  INT 5
  INC A0
  PUSH A0
  ; push_int(240)
  PUSH $00f0
  ; rnd
  POP A1
  MOV A0,(RAND)
  MOV A2,997
  MULU A2,A0
  IN A0,20
  ADD A2,A0
  MOV (RAND),A2
  MOVI A0,6
  INT 5
  INC A0
  PUSH A0
  ; push_int(1)
  PUSH $0001
  ; plot
  POP A4
  POP A2
  POP A1
  MOVI A0,2
  INT 4
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; put static
  POP A7
  MOV (i), A7
  JR main_249
main_284:
  ; set_integer_local(8,300)
  MOV	A3,A6
  ADD A3,16
  MOV (A3), $012c
main_289:
  ;; push_ref(hit)
  PUSH (hit)
  ; push_int(6)
  PUSH $0006
  ;; jump_cond_integer(main_1943,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_1943
  ; push_local_int(6)
  MOV	A3,A6
  ADDI A3,12
  PUSH (A3)
  ;; pop_local_var_int(11)
  POP A7
  MOV A3,A6
  ADD	A3,22
  MOV (A3),A7
  ; set_integer_local(12,0)
  MOV	A3,A6
  ADD A3,24
  MOV (A3), $0000
  ; set_integer_local(10,0)
  MOV	A3,A6
  ADD A3,20
  MOV (A3), $0000
  ; push_local_int(11)
  MOV	A3,A6
  ADD A3,22
  PUSH (A3)
  ; push_int(68)
  PUSH $0044
  ;; jump_cond_integer(main_331,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_331
  ; joy1
  IN A1,22
  NOT A1
  AND A1,31
  PUSH A1
  ;; stack_alu_const(AND, 8)
  POP A7
  AND A7,$0008
  PUSH A7
  ;; jump_cond(main_346, eq)
  POP A7
  CMPI A7,0
  JRZ main_346
  ; push_local_int(7)
  MOV	A3,A6
  ADDI A3,14
  PUSH (A3)
  ; push_int(16)
  PUSH $0010
  ;; jump_cond_integer(main_346,le)
  POP A4
  POP A7
  CMP A7,A4
  JRLE main_346
main_331:
  ; set_integer_local(10,1)
  MOV	A3,A6
  ADD A3,20
  MOV (A3), $0001
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; push_int(8)
  PUSH $0008
  POP A1
  POP A2
  MOVI A0,9
  INT 4
  PUSH A0
  ;; pop_local_var_int(5)
  POP A7
  MOV A3,A6
  ADDI A3,10
  MOV (A3),A7
  ; set_integer_local(12,1)
  MOV	A3,A6
  ADD A3,24
  MOV (A3), $0001
main_346:
  ; push_local_int(11)
  MOV	A3,A6
  ADD A3,22
  PUSH (A3)
  ; push_int(67)
  PUSH $0043
  ;; jump_cond_integer(main_369,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_369
  ; joy1
  IN A1,22
  NOT A1
  AND A1,31
  PUSH A1
  ;; stack_alu_const(AND, 16)
  POP A7
  AND A7,$0010
  PUSH A7
  ;; jump_cond(main_393, eq)
  POP A7
  CMPI A7,0
  JRZ main_393
  ; push_local_int(7)
  MOV	A3,A6
  ADDI A3,14
  PUSH (A3)
  ; push_int(16)
  PUSH $0010
  ;; jump_cond_integer(main_393,le)
  POP A4
  POP A7
  CMP A7,A4
  JRLE main_393
main_369:
  ; set_integer_local(10,1)
  MOV	A3,A6
  ADD A3,20
  MOV (A3), $0001
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ;; jump_cond(main_384, gt)
  POP A7
  CMPI A7,0
  JRG main_384
  ; set_integer_local(5,7)
  MOV	A3,A6
  ADDI A3,10
  MOV (A3), $0007
  JR main_390
main_384:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ;; stack_alu_const(SUB, 1)
  POP A5
  SUBI A5,1
  PUSH A5
  ;; pop_local_var_int(5)
  POP A7
  MOV A3,A6
  ADDI A3,10
  MOV (A3),A7
main_390:
  ; set_integer_local(12,1)
  MOV	A3,A6
  ADD A3,24
  MOV (A3), $0001
main_393:
  ; push_local_int(11)
  MOV	A3,A6
  ADD A3,22
  PUSH (A3)
  ; push_int(65)
  PUSH $0041
  ;; jump_cond_integer(main_415,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_415
  ; joy1
  IN A1,22
  NOT A1
  AND A1,31
  PUSH A1
  ;; stack_alu_const(AND, 2)
  POP A7
  AND A7,$0002
  PUSH A7
  ;; jump_cond(main_717, eq)
  POP A7
  CMPI A7,0
  JRZ main_717
  ; push_local_int(7)
  MOV	A3,A6
  ADDI A3,14
  PUSH (A3)
  ; push_int(16)
  PUSH $0010
  ;; jump_cond_integer(main_717,le)
  POP A4
  POP A7
  CMP A7,A4
  JRLE main_717
main_415:
  ; set_integer_local(10,1)
  MOV	A3,A6
  ADD A3,20
  MOV (A3), $0001
  ; push_int(11)
  PUSH $000b
  ; push_int(1)
  PUSH $0001
  ; out 
  POP A1
  POP A0
  OUT A0,A1
  ; push_int(0)
  PUSH $0000
  ; push_int(120)
  PUSH $0078
  ; push_int(1)
  PUSH $0001
  ; sound
  POP A3
  POP A2
  MOVI A1,0
  CMPI A2,0
  JRZ Label_1
  MULU A3,16384
  MOVI A1,4
  MOVI A0,9
  INT 4
  MOV A2,A1
  SUB A2,25000
  NEG A2
  MOVI A0,9
  INT 4
  ADD A1,A3
  LAbel_1: POP A0
  SLL A0,1
  ADDI A0,8
  OUT A0,A1
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ;; jump_cond(main_457, ne)
  POP A7
  CMPI A7,0
  JRNZ  main_457
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float -4.000000 = 49280 0
  PUSH $c080
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_717, le)
  POP A7
  CMPI A7,0
  JRLE main_717
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_717
main_457:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_502,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_502
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float -4.000000 = 49280 0
  PUSH $c080
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_481, le)
  POP A7
  CMPI A7,0
  JRLE main_481
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_481:
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float -4.000000 = 49280 0
  PUSH $c080
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_717, le)
  POP A7
  CMPI A7,0
  JRLE main_717
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_717
main_502:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(2)
  PUSH $0002
  ;; jump_cond_integer(main_529,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_529
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float -4.000000 = 49280 0
  PUSH $c080
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_717, le)
  POP A7
  CMPI A7,0
  JRLE main_717
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_717
main_529:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(3)
  PUSH $0003
  ;; jump_cond_integer(main_574,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_574
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float -4.000000 = 49280 0
  PUSH $c080
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_553, le)
  POP A7
  CMPI A7,0
  JRLE main_553
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_553:
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 4.000000 = 16512 0
  PUSH $4080
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_717, ge)
  POP A7
  CMPI A7,0
  JRGE main_717
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_717
main_574:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(4)
  PUSH $0004
  ;; jump_cond_integer(main_601,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_601
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 4.000000 = 16512 0
  PUSH $4080
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_717, ge)
  POP A7
  CMPI A7,0
  JRGE main_717
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_717
main_601:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(5)
  PUSH $0005
  ;; jump_cond_integer(main_646,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_646
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 4.000000 = 16512 0
  PUSH $4080
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_625, ge)
  POP A7
  CMPI A7,0
  JRGE main_625
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_625:
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 4.000000 = 16512 0
  PUSH $4080
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_717, ge)
  POP A7
  CMPI A7,0
  JRGE main_717
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_717
main_646:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(6)
  PUSH $0006
  ;; jump_cond_integer(main_674,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_674
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 4.000000 = 16512 0
  PUSH $4080
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_717, ge)
  POP A7
  CMPI A7,0
  JRGE main_717
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_717
main_674:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(7)
  PUSH $0007
  ;; jump_cond_integer(main_717,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_717
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 4.000000 = 16512 0
  PUSH $4080
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_699, ge)
  POP A7
  CMPI A7,0
  JRGE main_699
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_699:
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float -4.000000 = 49280 0
  PUSH $c080
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_717, le)
  POP A7
  CMPI A7,0
  JRLE main_717
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.200000 = 15948 52429
  PUSH $3e4c
  PUSH $cccd
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_717:
  ; push_local_int(11)
  MOV	A3,A6
  ADD A3,22
  PUSH (A3)
  ; push_int(32)
  PUSH $0020
  ;; jump_cond_integer(main_739,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_739
  ; joy1
  IN A1,22
  NOT A1
  AND A1,31
  PUSH A1
  ;; stack_alu_const(AND, 1)
  POP A7
  AND A7,$0001
  PUSH A7
  ;; jump_cond(main_945, eq)
  POP A7
  CMPI A7,0
  JRZ main_945
  ; push_local_int(7)
  MOV	A3,A6
  ADDI A3,14
  PUSH (A3)
  ; push_int(18)
  PUSH $0012
  ;; jump_cond_integer(main_945,le)
  POP A4
  POP A7
  CMP A7,A4
  JRLE main_945
main_739:
  ; set_integer_local(10,1)
  MOV	A3,A6
  ADD A3,20
  MOV (A3), $0001
  ; push_int(11)
  PUSH $000b
  ; push_int(1)
  PUSH $0001
  ; out 
  POP A1
  POP A0
  OUT A0,A1
  ; push_int(0)
  PUSH $0000
  ; push_int(3000)
  PUSH $0bb8
  ; push_int(2)
  PUSH $0002
  ; sound
  POP A3
  POP A2
  MOVI A1,0
  CMPI A2,0
  JRZ Label_2
  MULU A3,16384
  MOVI A1,4
  MOVI A0,9
  INT 4
  MOV A2,A1
  SUB A2,25000
  NEG A2
  MOVI A0,9
  INT 4
  ADD A1,A3
  LAbel_2: POP A0
  SLL A0,1
  ADDI A0,8
  OUT A0,A1
  ; get static float
  MOV A7, sx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; put static float
  POP A4
  POP A7
  MOV A5, bx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; get static float
  MOV A7, sy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; put static float
  POP A4
  POP A7
  MOV A5, by
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 250.000000 = 17274 0
  PUSH $437a
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, brange
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ;; jump_cond(main_790, ne)
  POP A7
  CMPI A7,0
  JRNZ  main_790
  ;push float -6.000000 = 49344 0
  PUSH $c0c0
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_919
main_790:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_809,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_809
  ;push float -3.000000 = 49216 0
  PUSH $c040
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float -3.000000 = 49216 0
  PUSH $c040
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_919
main_809:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(2)
  PUSH $0002
  ;; jump_cond_integer(main_827,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_827
  ;push float -4.000000 = 49280 0
  PUSH $c080
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_919
main_827:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(3)
  PUSH $0003
  ;; jump_cond_integer(main_846,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_846
  ;push float -3.000000 = 49216 0
  PUSH $c040
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 3.000000 = 16448 0
  PUSH $4040
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_919
main_846:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(4)
  PUSH $0004
  ;; jump_cond_integer(main_864,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_864
  ;push float 6.000000 = 16576 0
  PUSH $40c0
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_919
main_864:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(5)
  PUSH $0005
  ;; jump_cond_integer(main_883,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_883
  ;push float 3.000000 = 16448 0
  PUSH $4040
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 3.000000 = 16448 0
  PUSH $4040
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_919
main_883:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(6)
  PUSH $0006
  ;; jump_cond_integer(main_902,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_902
  ;push float 6.000000 = 16576 0
  PUSH $40c0
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_919
main_902:
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(7)
  PUSH $0007
  ;; jump_cond_integer(main_919,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_919
  ;push float 3.000000 = 16448 0
  PUSH $4040
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float -3.000000 = 49216 0
  PUSH $c040
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_919:
  ; get static float
  MOV A7, bdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 4.000000 = 16512 0
  PUSH $4080
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, bdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; get static float
  MOV A7, bdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 4.000000 = 16512 0
  PUSH $4080
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, bdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_945:
  ; push_local_int(10)
  MOV	A3,A6
  ADD A3,20
  PUSH (A3)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_957,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_957
  ; set_integer_local(7,0)
  MOV	A3,A6
  ADDI A3,14
  MOV (A3), $0000
  ; set_integer_local(10,0)
  MOV	A3,A6
  ADD A3,20
  MOV (A3), $0000
main_957:
  ; timer
  IN	A1,20
  BCLR A1,15
  PUSH A1
  ; push_local_int(3)
  MOV	A3,A6
  ADDI A3,6
  PUSH (A3)
  ;; stack_alu(SUB)
  POP A7
  POP A4
  SUB A4, A7
  PUSH A4
  ;; pop_local_var_int(4)
  POP A7
  MOV A3,A6
  ADDI A3,8
  MOV (A3),A7
  ; push_local_int(4)
  MOV	A3,A6
  ADDI A3,8
  PUSH (A3)
  ;; jump_cond(main_974, ge)
  POP A7
  CMPI A7,0
  JRGE main_974
  ; push_local_int(4)
  MOV	A3,A6
  ADDI A3,8
  PUSH (A3)
  POP A4
  NEG A4
  PUSH A4
  ;; pop_local_var_int(4)
  POP A7
  MOV A3,A6
  ADDI A3,8
  MOV (A3),A7
main_974:
  ; push_local_int(12)
  MOV	A3,A6
  ADD A3,24
  PUSH (A3)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_1012,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_1012
  ; set_integer_local(1,0)
  MOV	A3,A6
  ADDI A3,2
  MOV (A3), $0000
main_982:
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_int(32)
  PUSH $0020
  ;; jump_cond_integer(main_1012,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_1012
  ; push_int(63552)
  PUSH $f840
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ;; push_ref(sp1_data)
  PUSH (sp1_data)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; push_int(32)
  PUSH $0020
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,1
  ADD A7, A4
  PUSH (A7)
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ;; inc_integer(1,1)
  MOV	A3,A6
  ADDI A3,2
  ADD (A3),1
  JR main_982
main_1012:
  ; push_local_int(4)
  MOV	A3,A6
  ADDI A3,8
  PUSH (A3)
  ; push_int(9)
  PUSH $0009
  ;; jump_cond_integer(main_1935,le)
  POP A4
  POP A7
  CMP A7,A4
  JRLE main_1935
  ; push_local_int(7)
  MOV	A3,A6
  ADDI A3,14
  PUSH (A3)
  ; push_int(30)
  PUSH $001e
  ;; jump_cond_integer(main_1029,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_1029
  ;; inc_integer(7,1)
  MOV	A3,A6
  ADDI A3,14
  ADD (A3),1
main_1029:
  ; timer
  IN	A1,20
  BCLR A1,15
  PUSH A1
  ;; pop_local_var_int(3)
  POP A7
  MOV A3,A6
  ADDI A3,6
  MOV (A3),A7
  ; push_local_int(8)
  MOV	A3,A6
  ADD A3,16
  PUSH (A3)
  ;; jump_cond(main_1045, ne)
  POP A7
  CMPI A7,0
  JRNZ  main_1045
  ; set_integer_local(9,7)
  MOV	A3,A6
  ADD A3,18
  MOV (A3), $0007
  JR main_1048
main_1045:
  ; set_integer_local(9,1)
  MOV	A3,A6
  ADD A3,18
  MOV (A3), $0001
main_1048:
  ; push_int(0)
  PUSH $0000
  ; push_int(0)
  PUSH $0000
  ; push_int(1)
  PUSH $0001
  ; get static float
  MOV A7, sx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; get static float
  MOV A7, sy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; push_local_int(9)
  MOV	A3,A6
  ADD A3,18
  PUSH (A3)
  ;; invoke_static_method(set_sprite_IIIIII,6,1)
  ADD SP,12
  MOV A7,SP
  SUB SP,18
  SETX 5
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, 8
  SETX 5
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,18
  JRSR set_sprite_IIIIII
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (i), A7
main_1068:
  ;; push_ref(i)
  PUSH (i)
  ; push_int(8)
  PUSH $0008
  ;; jump_cond_integer(main_1319,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_1319
  ;; push_ref(rockon)
  PUSH (rockon)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,1
  ADD A7, A4
  PUSH (A7)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_1280,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_1280
  ; push_int(2)
  PUSH $0002
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_int(0)
  PUSH $0000
  ; push_int(1)
  PUSH $0001
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; push_int(6)
  PUSH $0006
  ;; invoke_static_method(set_sprite_IIIIII,6,1)
  ADD SP,12
  MOV A7,SP
  SUB SP,18
  SETX 5
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, 8
  SETX 5
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,18
  JRSR set_sprite_IIIIII
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rockdx)
  PUSH (rockdx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1147, gt)
  POP A7
  CMPI A7,0
  JRG main_1147
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ;push float 382.000000 = 17343 0
  PUSH $43bf
  PUSH $0000
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  JR main_1196
main_1147:
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rockdx)
  PUSH (rockdx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 382.000000 = 17343 0
  PUSH $43bf
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1179, lt)
  POP A7
  CMPI A7,0
  JRL main_1179
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  JR main_1196
main_1179:
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ; dup2()
  POP	A4
  POP  A5
  PUSH A5
  PUSH A4
  PUSH A5
  PUSH A4
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rockdx)
  PUSH (rockdx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
main_1196:
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rockdy)
  PUSH (rockdy)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1228, gt)
  POP A7
  CMPI A7,0
  JRG main_1228
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ;push float 245.000000 = 17269 0
  PUSH $4375
  PUSH $0000
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  JR main_1308
main_1228:
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rockdy)
  PUSH (rockdy)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 245.000000 = 17269 0
  PUSH $4375
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1260, lt)
  POP A7
  CMPI A7,0
  JRL main_1260
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  JR main_1308
main_1260:
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ; dup2()
  POP	A4
  POP  A5
  PUSH A5
  PUSH A4
  PUSH A5
  PUSH A4
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rockdy)
  PUSH (rockdy)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  JR main_1308
main_1280:
  ; push_int(2)
  PUSH $0002
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_int(0)
  PUSH $0000
  ; push_int(0)
  PUSH $0000
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; push_int(6)
  PUSH $0006
  ;; invoke_static_method(set_sprite_IIIIII,6,1)
  ADD SP,12
  MOV A7,SP
  SUB SP,18
  SETX 5
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, 8
  SETX 5
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,18
  JRSR set_sprite_IIIIII
main_1308:
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; put static
  POP A7
  MOV (i), A7
  JR main_1068
main_1319:
  ; get static float
  MOV A7, brange
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1598, le)
  POP A7
  CMPI A7,0
  JRLE main_1598
  ; get static float
  MOV A7, brange
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; mul float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,9
  INT 5
  PUSH A1
  PUSH A2
  ; get static float
  MOV A7, bdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; mul float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,9
  INT 5
  PUSH A1
  PUSH A2
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; sqrt
  POP A2
  POP A1
  JRSR sqrt
  PUSH A1
  PUSH A2
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, brange
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; push_int(1)
  PUSH $0001
  ; push_int(0)
  PUSH $0000
  ; push_int(1)
  PUSH $0001
  ; get static float
  MOV A7, bx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; get static float
  MOV A7, by
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; push_int(7)
  PUSH $0007
  ;; invoke_static_method(set_sprite_IIIIII,6,1)
  ADD SP,12
  MOV A7,SP
  SUB SP,18
  SETX 5
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, 8
  SETX 5
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,18
  JRSR set_sprite_IIIIII
  ; get static float
  MOV A7, bx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1388, ge)
  POP A7
  CMPI A7,0
  JRGE main_1388
  ;push float 382.000000 = 17343 0
  PUSH $43bf
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_1418
main_1388:
  ; get static float
  MOV A7, bx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 382.000000 = 17343 0
  PUSH $43bf
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1408, le)
  POP A7
  CMPI A7,0
  JRLE main_1408
  ;push float 1.000000 = 16256 0
  PUSH $3f80
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, bx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_1418
main_1408:
  ; get static float
  MOV A7, bx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, bx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_1418:
  ; get static float
  MOV A7, by
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1438, ge)
  POP A7
  CMPI A7,0
  JRGE main_1438
  ;push float 246.000000 = 17270 0
  PUSH $4376
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, by
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_1468
main_1438:
  ; get static float
  MOV A7, by
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 246.000000 = 17270 0
  PUSH $4376
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1458, le)
  POP A7
  CMPI A7,0
  JRLE main_1458
  ;push float 1.000000 = 16256 0
  PUSH $3f80
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, by
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_1468
main_1458:
  ; get static float
  MOV A7, by
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, bdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, by
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_1468:
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (i), A7
main_1472:
  ;; push_ref(i)
  PUSH (i)
  ; push_int(8)
  PUSH $0008
  ;; jump_cond_integer(main_1618,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_1618
  ; get static float
  MOV A7, bx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, fdistx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; get static float
  MOV A7, by
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, fdisty
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;; push_ref(rockon)
  PUSH (rockon)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,1
  ADD A7, A4
  PUSH (A7)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_1587,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_1587
  ; get static float
  MOV A7, fdistx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, fdistx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; mul float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,9
  INT 5
  PUSH A1
  PUSH A2
  ; get static float
  MOV A7, fdisty
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, fdisty
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; mul float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,9
  INT 5
  PUSH A1
  PUSH A2
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 12.000000 = 16704 0
  PUSH $4140
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1587, ge)
  POP A7
  CMPI A7,0
  JRGE main_1587
  ; push_int(11)
  PUSH $000b
  ; push_int(1)
  PUSH $0001
  ; out 
  POP A1
  POP A0
  OUT A0,A1
  ;; push_ref(rockon)
  PUSH (rockon)
  ;; push_ref(i)
  PUSH (i)
  ; push_int(0)
  PUSH $0000
  ;; array_write_short()
  POP A5
  POP A4
  POP A7
  SLL A4,1
  ADD A7,A4
  MOV (A7),A5
  ; push_int(0)
  PUSH $0000
  ; push_int(100)
  PUSH $0064
  ; push_int(0)
  PUSH $0000
  ; sound
  POP A3
  POP A2
  MOVI A1,0
  CMPI A2,0
  JRZ Label_3
  MULU A3,16384
  MOVI A1,4
  MOVI A0,9
  INT 4
  MOV A2,A1
  SUB A2,25000
  NEG A2
  MOVI A0,9
  INT 4
  ADD A1,A3
  LAbel_3: POP A0
  SLL A0,1
  ADDI A0,8
  OUT A0,A1
  ;; push_ref(score)
  PUSH (score)
  ;; push_ref(level)
  PUSH (level)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; put static
  POP A7
  MOV (score), A7
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, brange
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; push_int(1)
  PUSH $0001
  ; push_int(4)
  PUSH $0004
  ;; push_ref(score)
  PUSH (score)
  ;; print num
  POP A1
  POP A0
  POP A2
  JRSR print_num
main_1587:
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; put static
  POP A7
  MOV (i), A7
  JR main_1472
main_1598:
  ; push_int(1)
  PUSH $0001
  ; push_int(0)
  PUSH $0000
  ; push_int(0)
  PUSH $0000
  ; get static float
  MOV A7, bx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; get static float
  MOV A7, by
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; push_int(7)
  PUSH $0007
  ;; invoke_static_method(set_sprite_IIIIII,6,1)
  ADD SP,12
  MOV A7,SP
  SUB SP,18
  SETX 5
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, 8
  SETX 5
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,18
  JRSR set_sprite_IIIIII
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, brange
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_1618:
  ; set_integer_local(13,1)
  MOV	A3,A6
  ADD A3,26
  MOV (A3), $0001
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (i), A7
main_1625:
  ;; push_ref(i)
  PUSH (i)
  ; push_int(8)
  PUSH $0008
  ;; jump_cond_integer(main_1658,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_1658
  ;; push_ref(rockon)
  PUSH (rockon)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,1
  ADD A7, A4
  PUSH (A7)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_1647,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_1647
  ; set_integer_local(13,0)
  MOV	A3,A6
  ADD A3,26
  MOV (A3), $0000
main_1647:
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; put static
  POP A7
  MOV (i), A7
  JR main_1625
main_1658:
  ; push_local_int(13)
  MOV	A3,A6
  ADD A3,26
  PUSH (A3)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_1683,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_1683
  ;; push_ref(level)
  PUSH (level)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; put static
  POP A7
  MOV (level), A7
  ;; push_ref(level)
  PUSH (level)
  ;; invoke_static_method(init_I,1,1)
  ADD SP,2
  MOV A7,SP
  SUB SP,8
  SETX 0
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, -2
  SETX 0
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,8
  JRSR init_I
  ; set_integer_local(8,300)
  MOV	A3,A6
  ADD A3,16
  MOV (A3), $012c
main_1683:
  ; get static float
  MOV A7, sx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 382.000000 = 17343 0
  PUSH $43bf
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1703, lt)
  POP A7
  CMPI A7,0
  JRL main_1703
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_1733
main_1703:
  ; get static float
  MOV A7, sx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1723, gt)
  POP A7
  CMPI A7,0
  JRG main_1723
  ;push float 382.000000 = 17343 0
  PUSH $43bf
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_1733
main_1723:
  ; get static float
  MOV A7, sx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, sdx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_1733:
  ; get static float
  MOV A7, sy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 246.000000 = 17270 0
  PUSH $4376
  PUSH $0000
  ; compare floats 0
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1753, lt)
  POP A7
  CMPI A7,0
  JRL main_1753
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_1783
main_1753:
  ; get static float
  MOV A7, sy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1773, gt)
  POP A7
  CMPI A7,0
  JRG main_1773
  ;push float 246.000000 = 17270 0
  PUSH $4376
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  JR main_1783
main_1773:
  ; get static float
  MOV A7, sy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, sdy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, sy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_1783:
  ; push_local_int(8)
  MOV	A3,A6
  ADD A3,16
  PUSH (A3)
  ;; jump_cond(main_1932, ne)
  POP A7
  CMPI A7,0
  JRNZ  main_1932
  ; push_int(0)
  PUSH $0000
  ; put static
  POP A7
  MOV (i), A7
main_1792:
  ;; push_ref(i)
  PUSH (i)
  ; push_int(8)
  PUSH $0008
  ;; jump_cond_integer(main_1935,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE main_1935
  ; get static float
  MOV A7, sx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rockx)
  PUSH (rockx)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, fdistx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ; get static float
  MOV A7, sy
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ;; push_ref(rocky)
  PUSH (rocky)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; sub_float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOV A0,A3
  OR A0,A4
  JRZ 8
  XOR A3,$8000
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 2.000000 = 16384 0
  PUSH $4000
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
  ; put static float
  POP A4
  POP A7
  MOV A5, fdisty
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;; push_ref(rockon)
  PUSH (rockon)
  ;; push_ref(i)
  PUSH (i)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,1
  ADD A7, A4
  PUSH (A7)
  ; push_int(1)
  PUSH $0001
  ;; jump_cond_integer(main_1921,ne)
  POP A4
  POP A7
  CMP A7,A4
  JRNZ main_1921
  ; get static float
  MOV A7, fdistx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, fdistx
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; mul float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,9
  INT 5
  PUSH A1
  PUSH A2
  ; get static float
  MOV A7, fdisty
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; get static float
  MOV A7, fdisty
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  ; mul float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,9
  INT 5
  PUSH A1
  PUSH A2
  ; add float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  ;push float 32.000000 = 16896 0
  PUSH $4200
  PUSH $0000
  ; compare floats 1
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,12
  INT 5
  PUSH A0
  ;; jump_cond(main_1921, ge)
  POP A7
  CMPI A7,0
  JRGE main_1921
  ; push_int(11)
  PUSH $000b
  ; push_int(0)
  PUSH $0000
  ; out 
  POP A1
  POP A0
  OUT A0,A1
  ; push_int(0)
  PUSH $0000
  ; push_int(3000)
  PUSH $0bb8
  ; push_int(2)
  PUSH $0002
  ; sound
  POP A3
  POP A2
  MOVI A1,0
  CMPI A2,0
  JRZ Label_4
  MULU A3,16384
  MOVI A1,4
  MOVI A0,9
  INT 4
  MOV A2,A1
  SUB A2,25000
  NEG A2
  MOVI A0,9
  INT 4
  ADD A1,A3
  LAbel_4: POP A0
  SLL A0,1
  ADDI A0,8
  OUT A0,A1
  ;; push_ref(hit)
  PUSH (hit)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; put static
  POP A7
  MOV (hit), A7
  ; push_int(1)
  PUSH $0001
  ; push_int(58)
  PUSH $003a
  ; push_int(6)
  PUSH $0006
  ;; push_ref(hit)
  PUSH (hit)
  ;; stack_alu(SUB)
  POP A7
  POP A4
  SUB A4, A7
  PUSH A4
  ;; print num
  POP A1
  POP A0
  POP A2
  JRSR print_num
  ; set_integer_local(8,300)
  MOV	A3,A6
  ADD A3,16
  MOV (A3), $012c
  ;push float 192.000000 = 17216 0
  PUSH $4340
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 124.000000 = 17144 0
  PUSH $42f8
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sdx
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
  ;push float 0.000000 = 0 0
  PUSH $0000
  PUSH $0000
  ; put static float
  POP A4
  POP A7
  MOV A5, sdy
  MOV (A5),A7
  ADDI A5,2
  MOV (A5), A4
main_1921:
  ;; push_ref(i)
  PUSH (i)
  ;; stack_alu_const(ADD, 1)
  POP A5
  ADDI A5,1
  PUSH A5
  ; put static
  POP A7
  MOV (i), A7
  JR main_1792
main_1932:
  ;; inc_integer(8,-1)
  MOV	A3,A6
  ADD A3,16
  ADD (A3),-1
main_1935:
  ; inkey
  MOVI A1,0
  MOVI A0,0
  INT 4
  BTST A0,1
  JRNZ Label_5
  MOVI A0,7
  INT 4
  BTST A0,2
  JRZ	Label_5
  MOVI A0,10
  INT 4
Label_5:
  PUSH A1
  ;; pop_local_var_int(6)
  POP A7
  MOV A3,A6
  ADDI A3,12
  MOV (A3),A7
  JR main_289
main_1943:
  ; set_integer_local(6,0)
  MOV	A3,A6
  ADDI A3,12
  MOV (A3), $0000
  ; push_int(14)
  PUSH $000e
  ; push_int(22)
  PUSH $0016
  ;; push_ref_static(171)
  GADR A7, _string_171
  PUSH A7
  ;; putSTR
  POP A1
  POP A0
  POP A2
  MOVHL A2,A0
  MOVI A0,5
  INT 4
  ; push_int(16)
  PUSH $0010
  ; push_int(16)
  PUSH $0010
  ;; push_ref_static(172)
  GADR A7, _string_172
  PUSH A7
  ;; putSTR
  POP A1
  POP A0
  POP A2
  MOVHL A2,A0
  MOVI A0,5
  INT 4
  ;; push_ref(score)
  PUSH (score)
  ;; push_ref(hiscore)
  PUSH (hiscore)
  ;; jump_cond_integer(main_1988,le)
  POP A4
  POP A7
  CMP A7,A4
  JRLE main_1988
  ; push_int(18)
  PUSH $0012
  ; push_int(22)
  PUSH $0016
  ;; push_ref_static(173)
  GADR A7, _string_173
  PUSH A7
  ;; putSTR
  POP A1
  POP A0
  POP A2
  MOVHL A2,A0
  MOVI A0,5
  INT 4
  ;; push_ref(score)
  PUSH (score)
  ; put static
  POP A7
  MOV (hiscore), A7
main_1988:
  ; push_local_int(6)
  MOV	A3,A6
  ADDI A3,12
  PUSH (A3)
  ; push_int(81)
  PUSH $0051
  ;; jump_cond_integer(main_2017,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_2017
  ; push_local_int(6)
  MOV	A3,A6
  ADDI A3,12
  PUSH (A3)
  ; push_int(113)
  PUSH $0071
  ;; jump_cond_integer(main_2017,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_2017
  ; push_local_int(6)
  MOV	A3,A6
  ADDI A3,12
  PUSH (A3)
  ; push_int(13)
  PUSH $000d
  ;; jump_cond_integer(main_2017,eq)
  POP A4
  POP A7
  CMP A7,A4
  JRZ, main_2017
  ; inkey
  MOVI A1,0
  MOVI A0,0
  INT 4
  BTST A0,1
  JRNZ Label_6
  MOVI A0,7
  INT 4
  BTST A0,2
  JRZ	Label_6
  MOVI A0,10
  INT 4
Label_6:
  PUSH A1
  ;; pop_local_var_int(6)
  POP A7
  MOV A3,A6
  ADDI A3,12
  MOV (A3),A7
  JR main_1988
main_2017:
  JR main_9
main_2020:
  ; push_int(11)
  PUSH $000b
  ; push_int(0)
  PUSH $0000
  ; out 
  POP A1
  POP A0
  OUT A0,A1
  ;; return_void(14)
  ADD SP,28
  ; pop all
  POPX
  POP A7
  POP A6
  POP A5
  POP A4
  POP A3
  POP A2
  POP A1
  RET

XY_ DW 0
print_xy:
PUSH A2
PUSH A0
MOVR A2,(XY_)
MOVI A0,4
INT 4
CMP A2,63
JRZ 8
ADD A2,$0100
MOVR (XY_),A2
POP A0
POP A2
RET

print_num:
  MOVHL A2,A0
  MOVR (XY_),A2
PRTNUM:  MOVI A3,0
  BTST A1,15
  JRZ Label_7
  MOV A2,A1
  MOV A1,'-'
  JRSR print_xy
  MOV A1,A2
Label_7:
  MOV A2,A1
  MOVI A1,10
  MOVI A0,9
 INT 4
  PUSH A0
  INC A3
  CMPI A1,0
  JRNZ Label_7
  DEC A3
  SETX A3
Label_8:
  POP A1
  ADD A1,48
  JRSR print_xy
  JRX Label_8
  RET

print_unum:
  MOVHL A2,A0
  MOVR (XY_),A2
  MOVI A3,0
Label_9:
  MOV A2,A1
  MOVI A1,10
  MOVI A0,6
 INT 5
  PUSH A0
  INC A3
  CMPI A1,0
  JRNZ Label_9
  DEC A3
  SETX A3
Label_10:
  POP A1
  ADD A1,48
  JRSR print_xy
  JRX Label_10
  RET



float_to_int:
  MOV A5,A3
  MOV A2,A3
  AND A3,$007F
  BCLR A5,15
  SRL A5,7
  MOV A0,A3
  OR A0,A5
  OR A0,A4
  JRNZ 6
  MOVI A1,0
  JR FTOI_E
  BSET A3,7
  SUB A5,127
  MOV A1,$7FFF
  CMP A5,14
  JRG FTOI_E
  MOVI A1,1
  CMP A5,0
  JRL FTOI_E
FTOI_1:
  CMP A5,0
  JRZ FTOI_E
  SLL A1,1
  BTST A3,6
  JRZ 2
  BSET A1,0
  SLLL A3,A4
  DEC A5
  JR FTOI_1
FTOI_E:
  BTST A2,15
  JRZ 2
  NEG A1
  RET


float_to_long:
  MOV A5,A3
  MOV A7,A3
  AND A3,$007F
  BCLR A5,15
  SRL A5,7
  MOV A0,A3
  OR A0,A5
  OR A0,A4
  JRNZ 8
  MOVI A1,0
  MOVI A2,0
  JR FTOL_E
  BSET A3,7
  SUB A5,127
  MOV A1,$7FFF
  MOV A2,$FFFF
  CMP A5,30
  JRG FTOL_E
  MOVI A2,1
  MOVI A1,0
  CMP A5,0
  JRL FTOL_E
FTOL_1:
  CMP A5,0
  JRZ FTOL_E
  SLLL A1,A2
  BTST A3,6
  JRZ 2
  BSET A2,0
  SLLL A3,A4
  DEC A5
  JR FTOL_1
FTOL_E:
  BTST A7,15
  JRZ 8
  NOT A1
  NEG A2
  ADC A1,0
  RET


int_to_float:
  MOVI A5,0
  MOVI A3,0
  OR A4,A4
  JRZ ITOF_3
  BTST A4,15
  JRZ 6
  NEG A4
  MOV A3,$8000
  MOV A7,23+127
ITOF_1:
  BTST A5,7
  JRNZ ITOF_2
  SLLL A5,A4
  DEC A7
  JR ITOF_1
ITOF_2:
  BCLR A5,7
  SLL A7,7
  OR A5,A7
  OR A5,A3
ITOF_3:
  RET


long_to_float:
  MOVI A3,0
  MOVI A4,0
  MOVI A5,0
  MOV A7,A1
  OR A7,A2
  JRZ LTOF_3
  BTST A1,15
  JRZ 12
  NOT A1
  NEG A2
  ADC A1,0
  MOV A5,$8000
  MOVI A3,0
  MOVI A4,0
  AND A1,$00FF
  MOV A7,24+31+127
LTOF_1:
  BTST A3,7
  JRNZ LTOF_2
  SLLL A3,A4
  SLLL A1,A2
  ADC A4,0
  DEC A7
  JR LTOF_1
LTOF_2:
  BCLR A3,7
  SLL A7,7
  OR A3,A7
  OR A3,A5
LTOF_3:
  RET


expe:
  MOV A3,$3FB8
  MOV A4,$AA3B
  MOVI A0,9
  INT 5

exp2:
  PUSH A6
  MOVI A6,0
  BTST A1,15
  JRZ 4
  MOV A6,$3F80
  MOV A5,A1
  MOV A7,A2
  MOV A3,$42F2
  MOV A4,$8C55
  MOVI A0,11
  INT 5
  PUSH A1
  PUSH A2
  PUSH A5
  PUSH A7
  MOV A3,A5
  MOV A4,A7
  JRSR float_to_long
  JRSR long_to_float
  POP A2
  POP A1
  XOR A3,$8000
  MOVI A0,11
  INT 5
  MOV A3,A6
  MOVI A4,0
  MOVI A0,11
  INT 5
  POP A7
  POP A5
  PUSH A1
  PUSH A2
  MOV A1,$409A
  MOV A2,$F5F8
  POP A4
  POP A3
  PUSH A3
  PUSH A4
  XOR A3,$8000
  MOVI A0,11
  INT 5
  MOV A3,A1
  MOV A4,A2
  MOV A1,$41DD
  MOV A2,$D2FE
  MOVI A0,10
  INT 5
  MOV A3,A5
  MOV A4,A7
  MOVI A0,11
  INT 5
  MOV A5,A1
  MOV A7,A2
  POP A2
  POP A1
  MOV A3,$3FBE
  MOV A4,$BC8D
  MOVI A0,9
  INT 5
  MOV A3,A1
  MOV A4,A2
  XOR A3,$8000
  MOV A1,A5
  MOV A2,A7
  MOVI A0,11
  INT 5
  MOV A3,$4b00
  MOV A4,$0
  MOVI A0,9
  INT 5
  MOV A3,A1
  MOV A4,A2
  JRSR float_to_long
  POP A6
  RET

pow:
  PUSH A3
  PUSH A4

JRSR log2
  POP A4
  POP A3
  MOVI A0,9
  INT 5

JRSR exp2
  RET

log2:
  MOV A0,A1
  SRL A0,7
  MOV A7,A1
  AND A7,$0040
  JRZ LOG_1
  SUB A0,126
  AND A1,$007F
  OR A1,$3f00
  JR LOG_2
LOG_1:
  SUB A0,127
  AND A1,$007F
  OR A1,$3f80
LOG_2:
  PUSH A0
  MOV A3,$bf80
  MOVI A4,0
  MOVI A0,11
  INT 5
  MOV A5,A1
  MOV A7,A2
  MOV A3,$bf21
  MOV A4,$3248
  MOVI A0,9
  INT 5
  MOV A3,A5
  MOV A4,A7
  MOVI A0,9
  INT 5
  XCHG A5,A1
  XCHG A7,A2
  MOV A3,$3fbb
  MOV A4,$c593
  MOVI A0,9
  INT 5
  MOV A3,A5
  MOV A4,A7
  MOVI A0,11
  INT 5
  POP A4
  PUSH A1
  PUSH A2
  JRSR int_to_float
  MOV A3,A5
  POP A2
  POP A1
  MOVI A0,11
  INT 5
  RET

sqrt:
  MOV A5,A1
  MOV A7,A2
  MOV A3,$bf00
  MOVI A4,0
  MOVI A0,9
  INT 5
  XCHG A5,A1
  XCHG A7,A2
  SRLL A1,A2
  MOV A3,$5f37
  MOV A4,$5a86
  SUB A4,A2
  ADC A1,0
  SUB A3,A1
  XCHG A3,A5
  XCHG A4,A7
  MOV A2,A7
  MOV A1,A5
  MOVI A0,9
  INT 5
  MOV A4,A7
  MOV A3,A5
  MOVI A0,9
  INT 5
  MOV A3,$3fc0
  MOVI A4,0
  MOVI A0,11
  INT 5
  MOV A4,A7
  MOV A3,A5
  MOVI A0,9
  INT 5
  MOV A3,A1
  MOV A4,A2
  MOVI A2,0
  MOV A1,$3f80
  MOVI A0,10
  INT 5
  RET

set_sprite_IIIIII:
  PUSH A6
  MOV A3,SP
  SUB SP, 12
  MOV A6,SP
  ADDI A6,2
  ; push_int(63152)
  PUSH $f6b0
  ; push_int(128)
  PUSH $0080
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; push_int(8)
  PUSH $0008
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(3)
  MOV	A3,A6
  ADDI A3,6
  PUSH (A3)
  ; push_int(256)
  PUSH $0100
;  div16_int
  POP A1
  POP A2
  MOVI A0,9
  INT 4
  PUSH A1
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ; push_int(63153)
  PUSH $f6b1
  ; push_int(128)
  PUSH $0080
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; push_int(8)
  PUSH $0008
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(3)
  MOV	A3,A6
  ADDI A3,6
  PUSH (A3)
  ; push_int(256)
  PUSH $0100
  POP A1
  POP A2
  MOVI A0,9
  INT 4
  PUSH A0
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ; push_int(63154)
  PUSH $f6b2
  ; push_int(128)
  PUSH $0080
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; push_int(8)
  PUSH $0008
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_int(0)
  PUSH $0000
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ; push_int(63155)
  PUSH $f6b3
  ; push_int(128)
  PUSH $0080
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; push_int(8)
  PUSH $0008
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(4)
  MOV	A3,A6
  ADDI A3,8
  PUSH (A3)
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ; push_int(63158)
  PUSH $f6b6
  ; push_int(128)
  PUSH $0080
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; push_int(8)
  PUSH $0008
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(5)
  MOV	A3,A6
  ADDI A3,10
  PUSH (A3)
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ; push_int(63159)
  PUSH $f6b7
  ; push_int(128)
  PUSH $0080
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; push_int(8)
  PUSH $0008
  ;int mul
  POP A1
  POP A2
  MOVI A0,8
  INT 4
  PUSH A1
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_local_int(2)
  MOV	A3,A6
  ADDI A3,4
  PUSH (A3)
  ; outb 
  POP A1
  POP A0
  OUT.B A0,A1
  ;; return_void(6)
  ADD SP,12
  POP A6
  ; pop all
  RET

init_I:
  PUSH A6
  MOV A3,SP
  SUB SP, 4
  MOV A6,SP
  ADDI A6,2
  MOV A4,SP
  SUB SP,2
  SETX 0
  PUSH (A3)
  SUBI A3,2
  JRX -8
  SETX 0
  POP A5
  ADDI A4,2
  MOV (A4),A5
  JRX -10
  ADD SP,2
  ; set_integer_local(1,0)
  MOV	A3,A6
  ADDI A3,2
  MOV (A3), $0000
init_I_2:
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_int(8)
  PUSH $0008
  ;; jump_cond_integer(init_I_142,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE init_I_142
  ;; push_ref(rockon)
  PUSH (rockon)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_int(1)
  PUSH $0001
  ;; array_write_short()
  POP A5
  POP A4
  POP A7
  SLL A4,1
  ADD A7,A4
  MOV (A7),A5
  ;; push_ref(rockx)
  PUSH (rockx)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_int(20)
  PUSH $0014
  ; push_int(360)
  PUSH $0168
  ; rnd
  POP A1
  MOV A0,(RAND)
  MOV A2,997
  MULU A2,A0
  IN A0,20
  ADD A2,A0
  MOV (RAND),A2
  MOVI A0,6
  INT 5
  INC A0
  PUSH A0
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  POP A4
  JRSR int_to_float
  PUSH A5
  PUSH A4
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  ;; push_ref(rocky)
  PUSH (rocky)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_int(20)
  PUSH $0014
  ; push_int(210)
  PUSH $00d2
  ; rnd
  POP A1
  MOV A0,(RAND)
  MOV A2,997
  MULU A2,A0
  IN A0,20
  ADD A2,A0
  MOV (RAND),A2
  MOVI A0,6
  INT 5
  INC A0
  PUSH A0
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  POP A4
  JRSR int_to_float
  PUSH A5
  PUSH A4
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_int(4)
  PUSH $0004
  ;; jump_cond_integer(init_I_65,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE init_I_65
  ;; push_ref(rockdx)
  PUSH (rockdx)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; rnd
  POP A1
  MOV A0,(RAND)
  MOV A2,997
  MULU A2,A0
  IN A0,20
  ADD A2,A0
  MOV (RAND),A2
  MOVI A0,6
  INT 5
  INC A0
  PUSH A0
  POP A4
  JRSR int_to_float
  PUSH A5
  PUSH A4
  ;push float 3.000000 = 16448 0
  PUSH $4040
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  JR init_I_79
init_I_65:
  ;; push_ref(rockdx)
  PUSH (rockdx)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; rnd
  POP A1
  MOV A0,(RAND)
  MOV A2,997
  MULU A2,A0
  IN A0,20
  ADD A2,A0
  MOV (RAND),A2
  MOVI A0,6
  INT 5
  INC A0
  PUSH A0
  POP A4
  JRSR int_to_float
  PUSH A5
  PUSH A4
  ;push float 3.000000 = 16448 0
  PUSH $4040
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
; neg float
  POP A4
  POP A3
  XOR A3,$8000
  PUSH A3
  PUSH A4
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
init_I_79:
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_int(6)
  PUSH $0006
  ;; jump_cond_integer(init_I_101,ge)
  POP A4
  POP A7
  CMP A7,A4
  JRGE init_I_101
  ;; push_ref(rockdy)
  PUSH (rockdy)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; rnd
  POP A1
  MOV A0,(RAND)
  MOV A2,997
  MULU A2,A0
  IN A0,20
  ADD A2,A0
  MOV (RAND),A2
  MOVI A0,6
  INT 5
  INC A0
  PUSH A0
  POP A4
  JRSR int_to_float
  PUSH A5
  PUSH A4
  ;push float 3.000000 = 16448 0
  PUSH $4040
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
  JR init_I_115
init_I_101:
  ;; push_ref(rockdy)
  PUSH (rockdy)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ; push_local_int(0)
  MOV	A3,A6
  ADDI A3,0
  PUSH (A3)
  ; rnd
  POP A1
  MOV A0,(RAND)
  MOV A2,997
  MULU A2,A0
  IN A0,20
  ADD A2,A0
  MOV (RAND),A2
  MOVI A0,6
  INT 5
  INC A0
  PUSH A0
  POP A4
  JRSR int_to_float
  PUSH A5
  PUSH A4
  ;push float 3.000000 = 16448 0
  PUSH $4040
  PUSH $0000
  ; div float 
  POP A4
  POP A3
  POP A2
  POP A1
  MOVI A0,10
  INT 5
  PUSH A1
  PUSH A2
; neg float
  POP A4
  POP A3
  XOR A3,$8000
  PUSH A3
  PUSH A4
  ;; array_write_float()
  POP A3
  POP A5
  POP A4
  POP A7
  SLL A4,2
  ADD A7,A4
  MOV (A7),A5
  ADD A7,2
  MOV (A7),A3
init_I_115:
  ; push_int(2)
  PUSH $0002
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; stack_alu(ADD)
  POP A7
  POP A4
  ADD A4, A7
  PUSH A4
  ; push_int(0)
  PUSH $0000
  ; push_int(1)
  PUSH $0001
  ;; push_ref(rockx)
  PUSH (rockx)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ;; push_ref(rocky)
  PUSH (rocky)
  ; push_local_int(1)
  MOV	A3,A6
  ADDI A3,2
  PUSH (A3)
  ;; array_read_short()
  POP A4
  POP A7
  SLL A4,2
  ADD A7, A4
  PUSH (A7)
  ADDI A7,2
  PUSH (A7)
  POP A4
  POP A3
  JRSR float_to_int
  PUSH A1
  ; push_int(2)
  PUSH $0002
  ;; invoke_static_method(set_sprite_IIIIII,6,1)
  ADD SP,12
  MOV A7,SP
  SUB SP,18
  SETX 5
  PUSH (A7)
  SUBI A7,2
  JRX -8
  ADD A7, 8
  SETX 5
  POP A5
  MOV (A7),A5
  SUBI A7,2
  JRX -10
  ADD SP,18
  JRSR set_sprite_IIIIII
  ;; inc_integer(1,1)
  MOV	A3,A6
  ADDI A3,2
  ADD (A3),1
  JR init_I_2
init_I_142:
  ;; return_void(2)
  ADD SP,4
  POP A6
  ; pop all
  RET

  DW 38
_strMsg:
  TEXT "ASTRO Sprite Demo in Java by Leon 2018"
  DW 0
  DW 256   ; sp1_data.length
_sp1_data:
  dw $0000, $0000, $0001, $0080, $0002, $0040, $0002, $0040
  dw $0002, $0040, $0004, $0020, $0004, $0020, $0008, $0010
  dw $0008, $0010, $0008, $0010, $0010, $0008, $0010, $0008
  dw $0010, $0008, $001f, $00f8, $0006, $0060, $0000, $0000
  dw $0000, $0000, $0000, $0000, $0000, $001c, $0000, $0024
  dw $0001, $00c4, $0006, $0008, $001c, $0008, $0020, $0010
  dw $0040, $0010, $0020, $0010, $0070, $0020, $0028, $0040
  dw $0004, $0040, $000e, $0080, $0005, $0000, $0000, $0000
  dw $0000, $0000, $0000, $0000, $0000, $0000, $001e, $0000
  dw $0011, $00c0, $0030, $0030, $0030, $000e, $0010, $0001
  dw $0010, $0001, $0030, $000e, $0030, $0030, $0011, $00c0
  dw $001e, $0000, $0000, $0000, $0000, $0000, $0000, $0000
  dw $0000, $0000, $0002, $0080, $0007, $0040, $0002, $0020
  dw $0014, $0020, $0038, $0020, $0010, $0010, $0020, $0008
  dw $0030, $0008, $001c, $0008, $0002, $0004, $0001, $00c2
  dw $0000, $0022, $0000, $001e, $0000, $0000, $0000, $0000
  dw $0000, $0000, $0006, $0060, $001f, $00f8, $0010, $0008
  dw $0010, $0008, $0010, $0008, $0008, $0010, $0008, $0010
  dw $0008, $0010, $0004, $0020, $0004, $0020, $0002, $0040
  dw $0002, $0040, $0002, $0040, $0001, $0080, $0000, $0000
  dw $0000, $0000, $0000, $0080, $0001, $00e0, $0001, $0060
  dw $0002, $0030, $0002, $001c, $0004, $000c, $0008, $0006
  dw $0008, $0007, $0008, $000c, $0010, $0038, $0020, $00c0
  dw $0027, $0000, $003c, $0000, $0000, $0000, $0000, $0000
  dw $0000, $0000, $0000, $0000, $0000, $0000, $0000, $003c
  dw $0001, $00c4, $0006, $0006, $0038, $0006, $0040, $0004
  dw $0040, $0004, $0038, $0006, $0006, $0006, $0001, $00c4
  dw $0000, $003c, $0000, $0000, $0000, $0000, $0000, $0000
  dw $0000, $0000, $0000, $0000, $0000, $0000, $0038, $0000
  dw $0026, $0000, $0021, $0080, $0010, $0060, $0008, $0018
  dw $0008, $0004, $0004, $0002, $0004, $0004, $0004, $000e
  dw $0002, $0014, $0002, $0020, $0001, $0070, $0000, $00a0


  DW 32   ; dot_data.length
_dot_data:
  dw $0000, $0000, $0000, $0000, $0000, $0000, $0000, $0000
  dw $0000, $0000, $0000, $0000, $0000, $0000, $0001, $0080
  dw $0001, $0080, $0000, $0000, $0000, $0000, $0000, $0000
  dw $0000, $0000, $0000, $0000, $0000, $0000, $0000, $0000


  DW 32   ; rock_data.length
_rock_data:
  dw $0000, $0000, $0001, $00c0, $0006, $003c, $0018, $0004
  dw $0020, $0002, $0040, $0002, $0040, $0002, $0020, $0001
  dw $0020, $0001, $0010, $0002, $0010, $0002, $0010, $0084
  dw $000b, $0048, $000c, $0050, $0000, $0030, $0000, $0020


  DW 8   ; rockx.length
_rockx:
  DW  $0000, $0000, $0000, $0000, $0000, $0001, $0000, $00c0
  DW  $0000, $0006, $0000, $003c, $0000, $0018, $0000, $0004


  DW 8   ; rocky.length
_rocky:
  DW  $0000, $0000, $0000, $0000, $0000, $0001, $0000, $00c0
  DW  $0000, $0006, $0000, $003c, $0000, $0018, $0000, $0004


  DW 8   ; rockdx.length
_rockdx:
  DW  $0000, $0000, $0000, $0000, $0000, $0001, $0000, $00c0
  DW  $0000, $0006, $0000, $003c, $0000, $0018, $0000, $0004


  DW 8   ; rockdy.length
_rockdy:
  DW  $0000, $0000, $0000, $0000, $0000, $0001, $0000, $00c0
  DW  $0000, $0006, $0000, $003c, $0000, $0018, $0000, $0004


  DW 8   ; rockon.length
_rockon:
  dw $0000, $0000, $0001, $00c0, $0006, $003c, $0018, $0004


  DW 4
_string_144:
  TEXT "L :6"
  DW 0
  DW 4
_string_145:
  TEXT "S :0"
  DW 0
  DW 17
_string_171:
  TEXT "*** GAME OVER ***"
DB 0
  DW 25
_string_172:
  TEXT "PRESS ENTER TO PLAY AGAIN"
DB 0
  DW 18
_string_173:
  TEXT "!!! HIGH SCORE !!!"
  DW 0

