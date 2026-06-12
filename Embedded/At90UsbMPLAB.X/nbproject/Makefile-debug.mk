#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Include project Makefile
ifeq "${IGNORE_LOCAL}" "TRUE"
# do not include local makefile. User is passing all local related variables already
else
include Makefile
# Include makefile containing local settings
ifeq "$(wildcard nbproject/Makefile-local-debug.mk)" "nbproject/Makefile-local-debug.mk"
include nbproject/Makefile-local-debug.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=debug
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
IMAGE_TYPE=debug
OUTPUT_SUFFIX=elf
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
else
IMAGE_TYPE=production
OUTPUT_SUFFIX=hex
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
endif

ifeq ($(COMPARE_BUILD), true)
COMPARISON_BUILD=-mafrlcsj
else
COMPARISON_BUILD=
endif

# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}

# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Source Files Quoted if spaced
SOURCEFILES_QUOTED_IF_SPACED=src/daq_dev.c src/ringbuffer.c src/SUDD.c src/Timer2CTC.c src/usart_debug.c src/usart_drv.c src/usb_api.c src/usb_drv.c src/usb_isr.c src/usb_spec.c src/usb_ControlEndpoint.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/src/daq_dev.o ${OBJECTDIR}/src/ringbuffer.o ${OBJECTDIR}/src/SUDD.o ${OBJECTDIR}/src/Timer2CTC.o ${OBJECTDIR}/src/usart_debug.o ${OBJECTDIR}/src/usart_drv.o ${OBJECTDIR}/src/usb_api.o ${OBJECTDIR}/src/usb_drv.o ${OBJECTDIR}/src/usb_isr.o ${OBJECTDIR}/src/usb_spec.o ${OBJECTDIR}/src/usb_ControlEndpoint.o
POSSIBLE_DEPFILES=${OBJECTDIR}/src/daq_dev.o.d ${OBJECTDIR}/src/ringbuffer.o.d ${OBJECTDIR}/src/SUDD.o.d ${OBJECTDIR}/src/Timer2CTC.o.d ${OBJECTDIR}/src/usart_debug.o.d ${OBJECTDIR}/src/usart_drv.o.d ${OBJECTDIR}/src/usb_api.o.d ${OBJECTDIR}/src/usb_drv.o.d ${OBJECTDIR}/src/usb_isr.o.d ${OBJECTDIR}/src/usb_spec.o.d ${OBJECTDIR}/src/usb_ControlEndpoint.o.d

# Object Files
OBJECTFILES=${OBJECTDIR}/src/daq_dev.o ${OBJECTDIR}/src/ringbuffer.o ${OBJECTDIR}/src/SUDD.o ${OBJECTDIR}/src/Timer2CTC.o ${OBJECTDIR}/src/usart_debug.o ${OBJECTDIR}/src/usart_drv.o ${OBJECTDIR}/src/usb_api.o ${OBJECTDIR}/src/usb_drv.o ${OBJECTDIR}/src/usb_isr.o ${OBJECTDIR}/src/usb_spec.o ${OBJECTDIR}/src/usb_ControlEndpoint.o

# Source Files
SOURCEFILES=src/daq_dev.c src/ringbuffer.c src/SUDD.c src/Timer2CTC.c src/usart_debug.c src/usart_drv.c src/usb_api.c src/usb_drv.c src/usb_isr.c src/usb_spec.c src/usb_ControlEndpoint.c



CFLAGS=
ASFLAGS=
LDLIBSOPTIONS=

############# Tool locations ##########################################
# If you copy a project from one host to another, the path where the  #
# compiler is installed may be different.                             #
# If you open this project with MPLAB X in the new host, this         #
# makefile will be regenerated and the paths will be corrected.       #
#######################################################################
# fixDeps replaces a bunch of sed/cat/printf statements that slow down the build
FIXDEPS=fixDeps

.build-conf:  ${BUILD_SUBPROJECTS}
ifneq ($(INFORMATION_MESSAGE), )
	@echo $(INFORMATION_MESSAGE)
endif
	${MAKE}  -f nbproject/Makefile-debug.mk ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=AT90USB1287
# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/debug/c09ff0a816909c4146026f155f29effe6f5ae3af .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/debug/b05cda87e18db229b4b36ca2e38eb03d8bb7537c .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/debug/5a461b52d617441818ebb77e886c630ec5c6278c .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/debug/a0e601f7d7c75b527a231c415c88d69381173fdb .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/debug/cebdef184fef21a5413286ff012359d354120940 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/debug/dae44207103f094928773fa9d129e35ba0e97600 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/debug/40e10991ea910ae454f168522045fc41070da616 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/debug/d0a535de9ba6ebdd64dbb82718b2c9127157d7a8 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/debug/d05ad93883bb3e2a0b3c0f33916987d567e44ba5 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/debug/2548b667ef65f709008b55c78315763990803335 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c 
	
${OBJECTDIR}/src/usb_ControlEndpoint.o: src/usb_ControlEndpoint.c  .generated_files/flags/debug/f822251d4523e94d76de02f062c10a23daf983a6 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o.d 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT ${OBJECTDIR}/src/usb_ControlEndpoint.o -o ${OBJECTDIR}/src/usb_ControlEndpoint.o src/usb_ControlEndpoint.c 
	
else
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/debug/20435aaaf72de58b8ced44781ce490021683868a .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/debug/ff244995afe87f528d86562feea6e60cb6c4143e .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/debug/2df55c8d1315991e9df7fd9439ff51aab1a4d05e .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/debug/987e469bdc4d69ac608bfcdaea61da6765404037 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/debug/a279c62c968b4d8af8c54110f5292d05f9e88ce1 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/debug/e3e569f9f92805ad2297d0091e1467f3ae6dddf8 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/debug/d616fa6e05af4fd73eee087c58dca678a7503831 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/debug/5648670eadbaf72d8ca4f7fe36c496f52e0155db .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/debug/956e950bfcc433ec5621b756c5a1926a45e2d2e6 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/debug/ed7a7f32fcd48351f5e1de275e2dbd988169db2e .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c 
	
${OBJECTDIR}/src/usb_ControlEndpoint.o: src/usb_ControlEndpoint.c  .generated_files/flags/debug/610bffb652de6aad3fa7129f1b4acf6f10b69281 .generated_files/flags/debug/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o.d 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_debug=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT ${OBJECTDIR}/src/usb_ControlEndpoint.o -o ${OBJECTDIR}/src/usb_ControlEndpoint.o src/usb_ControlEndpoint.c 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assembleWithPreprocess
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    
	@${MKDIR} ${DISTDIR} 
	${MP_CC} $(MP_EXTRA_LD_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -Wl,-Map=${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.map  -D__DEBUG=1  -DXPRJ_debug=$(CND_CONF)  -Wl,--defsym=__MPLAB_BUILD=1   -mdfp="${DFP_DIR}/xc8"   -gdwarf-2 -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -gdwarf-3 -mno-const-data-in-progmem     $(COMPARISON_BUILD) -Wl,--memorysummary,${DISTDIR}/memoryfile.xml -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}      -Wl,--start-group  -Wl,-lm -Wl,--end-group  -Wl,--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1
	@${RM} ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.hex 
	
	
else
${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} ${DISTDIR} 
	${MP_CC} $(MP_EXTRA_LD_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -Wl,-Map=${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.map  -DXPRJ_debug=$(CND_CONF)  -Wl,--defsym=__MPLAB_BUILD=1   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O0 -Og -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -gdwarf-3 -mno-const-data-in-progmem     $(COMPARISON_BUILD) -Wl,--memorysummary,${DISTDIR}/memoryfile.xml -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}      -Wl,--start-group  -Wl,-lm -Wl,--end-group 
	${MP_CC_DIR}\\avr-objcopy -O ihex "${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}" "${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.hex"
	
endif


# Subprojects
.build-subprojects:


# Subprojects
.clean-subprojects:

# Clean Targets
.clean-conf: ${CLEAN_SUBPROJECTS}
	${RM} -r ${OBJECTDIR}
	${RM} -r ${DISTDIR}

# Enable dependency checking
.dep.inc: .depcheck-impl

DEPFILES=$(wildcard ${POSSIBLE_DEPFILES})
ifneq (${DEPFILES},)
include ${DEPFILES}
endif
