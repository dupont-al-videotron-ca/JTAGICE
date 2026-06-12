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
ifeq "$(wildcard nbproject/Makefile-local-release.mk)" "nbproject/Makefile-local-release.mk"
include nbproject/Makefile-local-release.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=release
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
	${MAKE}  -f nbproject/Makefile-release.mk ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=AT90USB1287
# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/release/ce0f1d1428fb0357f50a9a9597514b86b7c686d4 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/release/fc7e6c8761be10415fc51a3b7b15790ae8a2434d .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/release/ccd21b5338fecfe1a9e306710109f71f40abacfd .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/release/ce950db47cc152c029c5898f1b03f9f00be1b799 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/release/f4664c4c3bcb77557ab89368e318a65f7160c865 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/release/e3ba368df640dc891d22cba473cd637ed63180ec .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/release/b0438518613ff155eb04f9c8509bd6de5c3aea04 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/release/bbf1ff932a2bba9bc8342eb386f3af5d8b4bbfed .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/release/b923055b51d27b7bcd6a62257589820754e8f2fe .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c 
	
${OBJECTDIR}/src/usb_requests.o: src/usb_requests.c  .generated_files/flags/release/9a04895893a9e586bfb85eb9562cda13801b36f7 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_requests.o.d 
	@${RM} ${OBJECTDIR}/src/usb_requests.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_requests.o.d" -MT "${OBJECTDIR}/src/usb_requests.o.d" -MT ${OBJECTDIR}/src/usb_requests.o -o ${OBJECTDIR}/src/usb_requests.o src/usb_requests.c 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/release/c8e6403853e29a6204f32b488e8e795921bd598d .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1 -g -DDEBUG  -gdwarf-2  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c 
	
else
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/release/a273c5c1af11c6bcc2abe571095076ea14dd42bf .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/release/d9a0e3f73dfa2588fbd4c8cd4435997ebf0318b .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/release/b0390ce4addb2f0b078eee94bafa4a4419ec7d08 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/release/c8d7719ee99595c5ec269ebbd6e08482782d5dfd .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/release/d3487c6529a555e30fc7d6598a839be8a765ed3f .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/release/8338ea36d1461a2c32cd953b7ea874f05ddba1c .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/release/cbcdaa57c373d8236981e8922116f357424400e4 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/release/8c588e3b93f28574d6fdbd92455f1953de20de78 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/release/c39acbb99da7c7851b23ffd8348d9a677ef6de56 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c 
	
${OBJECTDIR}/src/usb_requests.o: src/usb_requests.c  .generated_files/flags/release/a829c2dc9364ace250dd5a82cbce98ea38b40a45 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_requests.o.d 
	@${RM} ${OBJECTDIR}/src/usb_requests.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_requests.o.d" -MT "${OBJECTDIR}/src/usb_requests.o.d" -MT ${OBJECTDIR}/src/usb_requests.o -o ${OBJECTDIR}/src/usb_requests.o src/usb_requests.c 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/release/dd667b4d7f8b88cf9067395478b7c79d49587232 .generated_files/flags/release/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -x c -D__$(MP_PROCESSOR_OPTION)__   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -DXPRJ_release=$(CND_CONF)  $(COMPARISON_BUILD)  -gdwarf-3 -mno-const-data-in-progmem     -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c 
	
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
	${MP_CC} $(MP_EXTRA_LD_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -Wl,-Map=${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.map  -D__DEBUG=1  -DXPRJ_release=$(CND_CONF)  -Wl,--defsym=__MPLAB_BUILD=1   -mdfp="${DFP_DIR}/xc8"   -gdwarf-2 -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -gdwarf-3 -mno-const-data-in-progmem     $(COMPARISON_BUILD) -Wl,--memorysummary,${DISTDIR}/memoryfile.xml -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}      -Wl,--start-group  -Wl,-lm -Wl,--end-group  -Wl,--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1
	@${RM} ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.hex 
	
	
else
${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} ${DISTDIR} 
	${MP_CC} $(MP_EXTRA_LD_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -Wl,-Map=${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.map  -DXPRJ_release=$(CND_CONF)  -Wl,--defsym=__MPLAB_BUILD=1   -mdfp="${DFP_DIR}/xc8"  -Wl,--gc-sections -O1 -ffunction-sections -fdata-sections -fshort-enums -fno-common -funsigned-char -funsigned-bitfields -Wall -gdwarf-3 -mno-const-data-in-progmem     $(COMPARISON_BUILD) -Wl,--memorysummary,${DISTDIR}/memoryfile.xml -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}      -Wl,--start-group  -Wl,-lm -Wl,--end-group 
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
