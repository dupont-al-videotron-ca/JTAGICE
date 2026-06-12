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
ifeq "$(wildcard nbproject/Makefile-local-debug_CPP.mk)" "nbproject/Makefile-local-debug_CPP.mk"
include nbproject/Makefile-local-debug_CPP.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=debug_CPP
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
COMPARISON_BUILD=
else
COMPARISON_BUILD=
endif

# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}

# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Source Files Quoted if spaced
SOURCEFILES_QUOTED_IF_SPACED=scr++/ClassRingbuffer.cpp src/daq_dev.c src/ringbuffer.c src/SUDD.c src/Timer2CTC.c src/usart_debug.c src/usart_drv.c src/usb_api.c src/usb_drv.c src/usb_isr.c src/usb_spec.c src/usb_ControlEndpoint.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/scr++/ClassRingbuffer.o ${OBJECTDIR}/src/daq_dev.o ${OBJECTDIR}/src/ringbuffer.o ${OBJECTDIR}/src/SUDD.o ${OBJECTDIR}/src/Timer2CTC.o ${OBJECTDIR}/src/usart_debug.o ${OBJECTDIR}/src/usart_drv.o ${OBJECTDIR}/src/usb_api.o ${OBJECTDIR}/src/usb_drv.o ${OBJECTDIR}/src/usb_isr.o ${OBJECTDIR}/src/usb_spec.o ${OBJECTDIR}/src/usb_ControlEndpoint.o
POSSIBLE_DEPFILES=${OBJECTDIR}/scr++/ClassRingbuffer.o.d ${OBJECTDIR}/src/daq_dev.o.d ${OBJECTDIR}/src/ringbuffer.o.d ${OBJECTDIR}/src/SUDD.o.d ${OBJECTDIR}/src/Timer2CTC.o.d ${OBJECTDIR}/src/usart_debug.o.d ${OBJECTDIR}/src/usart_drv.o.d ${OBJECTDIR}/src/usb_api.o.d ${OBJECTDIR}/src/usb_drv.o.d ${OBJECTDIR}/src/usb_isr.o.d ${OBJECTDIR}/src/usb_spec.o.d ${OBJECTDIR}/src/usb_ControlEndpoint.o.d

# Object Files
OBJECTFILES=${OBJECTDIR}/scr++/ClassRingbuffer.o ${OBJECTDIR}/src/daq_dev.o ${OBJECTDIR}/src/ringbuffer.o ${OBJECTDIR}/src/SUDD.o ${OBJECTDIR}/src/Timer2CTC.o ${OBJECTDIR}/src/usart_debug.o ${OBJECTDIR}/src/usart_drv.o ${OBJECTDIR}/src/usb_api.o ${OBJECTDIR}/src/usb_drv.o ${OBJECTDIR}/src/usb_isr.o ${OBJECTDIR}/src/usb_spec.o ${OBJECTDIR}/src/usb_ControlEndpoint.o

# Source Files
SOURCEFILES=scr++/ClassRingbuffer.cpp src/daq_dev.c src/ringbuffer.c src/SUDD.c src/Timer2CTC.c src/usart_debug.c src/usart_drv.c src/usb_api.c src/usb_drv.c src/usb_isr.c src/usb_spec.c src/usb_ControlEndpoint.c

# Pack Options 
PACK_COMPILER_OPTIONS=-I "${DFP_DIR}/include"
PACK_COMMON_OPTIONS=-B "${DFP_DIR}/gcc/dev/at90usb1287"



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
	${MAKE}  -f nbproject/Makefile-debug_CPP.mk ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=AT90USB1287
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
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/debug_CPP/ff174f7ede73d526b814bed857957f44ac10bde9 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o  -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/debug_CPP/6cb44311d55e195459daf82791e8c369bc02c643 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o  -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/debug_CPP/77fa7536502cdf657126deff8aebd0f8626bc9de .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o  -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/debug_CPP/f0473ba82c114ee5217c61362bdb318bbaceb634 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o  -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/debug_CPP/79159e75d504569bb5a987cd3bd28adc1b1aadac .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o  -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/debug_CPP/9c5efa239e9bbe7d3fd6eaa0c6b712ec5c65295d .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o  -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/debug_CPP/1d0a5a887b9b8ddf02b06d2ff407eb482ed0d754 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o  -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/debug_CPP/97f2b187075a8c925c27d1b48fc2924eed106753 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o  -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/debug_CPP/75171fd26f05954c6849a9832f822f0d97637aa7 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o  -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/debug_CPP/329c5c879be45050b06f30faddcd124f769243e3 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o  -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_ControlEndpoint.o: src/usb_ControlEndpoint.c  .generated_files/flags/debug_CPP/4c4e6463574a0db58074dc99477a32626c86a093 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o.d 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT ${OBJECTDIR}/src/usb_ControlEndpoint.o  -o ${OBJECTDIR}/src/usb_ControlEndpoint.o src/usb_ControlEndpoint.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
else
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/debug_CPP/37f7877135ac39d7eec85090190e01ea54e7eb46 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o  -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/debug_CPP/76f81afeb9bdea97030da05741a17b2717d91cd0 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o  -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/debug_CPP/9ea81425de5fe131bbb817199a97d15045fa3172 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o  -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/debug_CPP/5dd9667f11582d1f75858b686761e5a37906e060 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o  -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/debug_CPP/60f87fa620f4cf753f6181677b5fd605219b460f .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o  -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/debug_CPP/bacf4d4ea87fd1adb40aab1c2b4c773212b0a40c .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o  -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/debug_CPP/a2007bf179098bc1cec232c19e0a0f38d8480b94 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o  -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/debug_CPP/b782cc59fcbb6c625ca5437845d3c58e21c2cdd7 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o  -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/debug_CPP/5580ea1461da3ff78604434695199409c27eb74b .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o  -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/debug_CPP/ade1c3fb6001b1fafa4088bf90306a2f591e263e .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o  -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_ControlEndpoint.o: src/usb_ControlEndpoint.c  .generated_files/flags/debug_CPP/f6d4b648f326c9a79f5a533bb5d71154ebbab392 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o.d 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT ${OBJECTDIR}/src/usb_ControlEndpoint.o  -o ${OBJECTDIR}/src/usb_ControlEndpoint.o src/usb_ControlEndpoint.c  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compileCPP
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/scr++/ClassRingbuffer.o: scr++/ClassRingbuffer.cpp  .generated_files/flags/debug_CPP/313c301bae4e6db3781aba878fa11e23c60733f5 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/scr++" 
	@${RM} ${OBJECTDIR}/scr++/ClassRingbuffer.o.d 
	@${RM} ${OBJECTDIR}/scr++/ClassRingbuffer.o 
	 ${MP_CPPC} $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c++ -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -I "src" -I "scr++" -Wall -MD -MP -MF "${OBJECTDIR}/scr++/ClassRingbuffer.o.d" -MT "${OBJECTDIR}/scr++/ClassRingbuffer.o.d" -MT ${OBJECTDIR}/scr++/ClassRingbuffer.o  -o ${OBJECTDIR}/scr++/ClassRingbuffer.o scr++/ClassRingbuffer.cpp  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
else
${OBJECTDIR}/scr++/ClassRingbuffer.o: scr++/ClassRingbuffer.cpp  .generated_files/flags/debug_CPP/8d4d025ecb6bb63ec7c2c664393387c4f66f5415 .generated_files/flags/debug_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/scr++" 
	@${RM} ${OBJECTDIR}/scr++/ClassRingbuffer.o.d 
	@${RM} ${OBJECTDIR}/scr++/ClassRingbuffer.o 
	 ${MP_CPPC} $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c++ -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O0 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -I "src" -I "scr++" -Wall -MD -MP -MF "${OBJECTDIR}/scr++/ClassRingbuffer.o.d" -MT "${OBJECTDIR}/scr++/ClassRingbuffer.o.d" -MT ${OBJECTDIR}/scr++/ClassRingbuffer.o  -o ${OBJECTDIR}/scr++/ClassRingbuffer.o scr++/ClassRingbuffer.cpp  -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    
	@${MKDIR} ${DISTDIR} 
	${MP_CPPC} $(MP_EXTRA_LD_PRE) -mmcu=at90usb1287 ${PACK_COMMON_OPTIONS}   -gdwarf-2 -D__$(MP_PROCESSOR_OPTION)__  -Wl,-Map="${DISTDIR}\At90UsbMPLAB.X.${IMAGE_TYPE}.map"    -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}      -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD)  -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=__ICD2RAM=1,--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1 -Wl,--gc-sections -Wl,--start-group  -Wl,-lm -Wl,--end-group 
	
	
	
	
	
	
else
${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} ${DISTDIR} 
	${MP_CPPC} $(MP_EXTRA_LD_PRE) -mmcu=at90usb1287 ${PACK_COMMON_OPTIONS}  -D__$(MP_PROCESSOR_OPTION)__  -Wl,-Map="${DISTDIR}\At90UsbMPLAB.X.${IMAGE_TYPE}.map"    -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}      -DXPRJ_debug_CPP=$(CND_CONF)  $(COMPARISON_BUILD)  -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION) -Wl,--gc-sections -Wl,--start-group  -Wl,-lm -Wl,--end-group 
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
