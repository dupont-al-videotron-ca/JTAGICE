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
ifeq "$(wildcard nbproject/Makefile-local-default.mk)" "nbproject/Makefile-local-default.mk"
include nbproject/Makefile-local-default.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=default
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
SOURCEFILES_QUOTED_IF_SPACED=src/daq_dev.c src/ringbuffer.c src/SUDD.c src/Timer2CTC.c src/usart_debug.c src/usart_drv.c src/usb_api.c src/usb_drv.c src/usb_isr.c src/usb_requests.c src/usb_spec.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/src/daq_dev.o ${OBJECTDIR}/src/ringbuffer.o ${OBJECTDIR}/src/SUDD.o ${OBJECTDIR}/src/Timer2CTC.o ${OBJECTDIR}/src/usart_debug.o ${OBJECTDIR}/src/usart_drv.o ${OBJECTDIR}/src/usb_api.o ${OBJECTDIR}/src/usb_drv.o ${OBJECTDIR}/src/usb_isr.o ${OBJECTDIR}/src/usb_requests.o ${OBJECTDIR}/src/usb_spec.o
POSSIBLE_DEPFILES=${OBJECTDIR}/src/daq_dev.o.d ${OBJECTDIR}/src/ringbuffer.o.d ${OBJECTDIR}/src/SUDD.o.d ${OBJECTDIR}/src/Timer2CTC.o.d ${OBJECTDIR}/src/usart_debug.o.d ${OBJECTDIR}/src/usart_drv.o.d ${OBJECTDIR}/src/usb_api.o.d ${OBJECTDIR}/src/usb_drv.o.d ${OBJECTDIR}/src/usb_isr.o.d ${OBJECTDIR}/src/usb_requests.o.d ${OBJECTDIR}/src/usb_spec.o.d

# Object Files
OBJECTFILES=${OBJECTDIR}/src/daq_dev.o ${OBJECTDIR}/src/ringbuffer.o ${OBJECTDIR}/src/SUDD.o ${OBJECTDIR}/src/Timer2CTC.o ${OBJECTDIR}/src/usart_debug.o ${OBJECTDIR}/src/usart_drv.o ${OBJECTDIR}/src/usb_api.o ${OBJECTDIR}/src/usb_drv.o ${OBJECTDIR}/src/usb_isr.o ${OBJECTDIR}/src/usb_requests.o ${OBJECTDIR}/src/usb_spec.o

# Source Files
SOURCEFILES=src/daq_dev.c src/ringbuffer.c src/SUDD.c src/Timer2CTC.c src/usart_debug.c src/usart_drv.c src/usb_api.c src/usb_drv.c src/usb_isr.c src/usb_requests.c src/usb_spec.c



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
	${MAKE}  -f nbproject/Makefile-default.mk ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=AT90USB1287
# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/default/407486bd904f028e908f3d703900a2a151c15ddf .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/default/85a9ff7256e6d79f880b7a59e9fe5c352efccd2 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/default/21a94776eb8d11e6eed3b554ea4e0b0d12bfc76 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/default/9846192635de77802504f00fa98d8fc45afed9e7 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/default/3ddcc594e83ffb28f8b94f08a019ad826ac01c37 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/default/d50d484fd8d43a22d5c926560e4c7c2fce1b9ce2 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/default/24098fb591c663770a254458459aeb63de165ce2 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/default/8279bc1e9fa35b243803dea2b544a98c59e74406 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/default/c408ac0fa9cc547f50e170e9ac13fce5869d30e .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c 
	
${OBJECTDIR}/src/usb_requests.o: src/usb_requests.c  .generated_files/flags/default/db3ee3bc8093cd905798ac3ecc95642005ef3b91 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_requests.o.d 
	@${RM} ${OBJECTDIR}/src/usb_requests.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_requests.o.d" -MT "${OBJECTDIR}/src/usb_requests.o.d" -MT ${OBJECTDIR}/src/usb_requests.o -o ${OBJECTDIR}/src/usb_requests.o src/usb_requests.c 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/default/fa475d30d7bd8b488b0bd2a26ad360710fdeb62b .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c 
	
else
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/default/ce8da8583d5004d930179d362320827a9bbcf25b .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/default/6fbb5c43f7035f6150146c7a5d953d1fab7ca9fb .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/default/d9139c10afbf3eeed9bda1f9aadfd26c1b3436a5 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/default/b645779a34a70bf4cda3c0f3ccb177cc9909c337 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/default/19cccdec2ddbeb9c38e78fd865cd421ef04f5637 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/default/951da6119e495011988f4cf570a704ea522383a3 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/default/e038b1c4832b1875d2c03c7a6cfbfe7dfeaff9ca .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/default/dc0a14c4b4378f6b3e43f31dd897da4156354b4e .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/default/ba30118eb75a919607bea112a4dd3b4b7c8064d8 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c 
	
${OBJECTDIR}/src/usb_requests.o: src/usb_requests.c  .generated_files/flags/default/88473edbafa394b5d185679193dbb9118aa7d6b6 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_requests.o.d 
	@${RM} ${OBJECTDIR}/src/usb_requests.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_requests.o.d" -MT "${OBJECTDIR}/src/usb_requests.o.d" -MT ${OBJECTDIR}/src/usb_requests.o -o ${OBJECTDIR}/src/usb_requests.o src/usb_requests.c 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/default/dce1aa80406170ab4ddb4ea69ed824ba61e98166 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c 
	
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
	${MP_CC} $(MP_EXTRA_LD_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -Wl,-Map=${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.map  -D__DEBUG=1  -DXPRJ_default=$(CND_CONF)  -Wl,--defsym=__MPLAB_BUILD=1   -mdfp="${DFP_DIR}/xc8"   -gdwarf-2 -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -gdwarf-3 -mno-const-data-in-progmem     $(COMPARISON_BUILD) -Wl,--memorysummary,${DISTDIR}/memoryfile.xml -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}      -Wl,--start-group  -Wl,-lm -Wl,--end-group  -Wl,--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1
	@${RM} ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.hex 
	
	
else
${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} ${DISTDIR} 
	${MP_CC} $(MP_EXTRA_LD_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -Wl,-Map=${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.map  -DXPRJ_default=$(CND_CONF)  -Wl,--defsym=__MPLAB_BUILD=1   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -gdwarf-3 -mno-const-data-in-progmem     $(COMPARISON_BUILD) -Wl,--memorysummary,${DISTDIR}/memoryfile.xml -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}      -Wl,--start-group  -Wl,-lm -Wl,--end-group 
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
