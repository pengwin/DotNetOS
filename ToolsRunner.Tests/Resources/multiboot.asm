use32
UMB_START = 0x00100000
org UMB_START

MB_MAGIC = 1BADB002h
MB_F_BOOTALIGNED	= 1
MB_F_MEMINFO		= (1 shl 1)
MB_F_VIDEOTABLE		= (1 shl 2)
MB_F_USE_MBOFFSETS	= (1 shl 16)
MB_FLAGS = MB_F_BOOTALIGNED or MB_F_MEMINFO or \
	   MB_F_USE_MBOFFSETS
MB_CHECKSUM = dword - (MB_MAGIC + MB_FLAGS)
VIDEO_MODE = 1 ; EGA text mode
VIDEO_WIDTH = 80
VIDEO_HEIGHT = 25
VIDEO_DEPTH = 0
VIDEO_BASE_ADDR = 0xB8000

multiboot_header:
		dd MB_MAGIC		; Multiboot magic number
flags:	   	dd MB_FLAGS
checksum:	dd MB_CHECKSUM
header_addr:	dd multiboot_header
load_addr:	dd multiboot_header	; start of program text
load_end_addr:	dd load_end		; end of program text+data
bss_end_addr:	dd bss+10000h
entry_addr:	dd entry_point		; 
mode_type:	dd VIDEO_MODE		; Video mode
width:		dd VIDEO_WIDTH		; Video Width
height:		dd VIDEO_HEIGHT		; Video Height
depth:		dd VIDEO_DEPTH		; Vide Depth


entry_point:
	cmp eax,0x2BADB002	; Are we multibooted
	hlt			; not really sure how we'd ever get here
	jmp $-1
fail:
	hlt
load_end:

bss:
mbis:	rd 0
umb_end: rd 0