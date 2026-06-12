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
ifeq "$(wildcard nbproject/Makefile-local-release_CPP.mk)" "nbproject/Makefile-local-release_CPP.mk"
include nbproject/Makefile-local-release_CPP.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=release_CPP
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
SOURCEFILES_QUOTED_IF_SPACED=src/daq_dev.c src/ringbuffer.c src/SUDD.c src/Timer2CTC.c src/usart_debug.c src/usart_drv.c src/usb_api.c src/usb_drv.c src/usb_isr.c src/usb_spec.c src/usb_ControlEndpoint.c scr++/ClassRingbuffer.cpp

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/src/daq_dev.o ${OBJECTDIR}/src/ringbuffer.o ${OBJECTDIR}/src/SUDD.o ${OBJECTDIR}/src/Timer2CTC.o ${OBJECTDIR}/src/usart_debug.o ${OBJECTDIR}/src/usart_drv.o ${OBJECTDIR}/src/usb_api.o ${OBJECTDIR}/src/usb_drv.o ${OBJECTDIR}/src/usb_isr.o ${OBJECTDIR}/src/usb_spec.o ${OBJECTDIR}/src/usb_ControlEndpoint.o ${OBJECTDIR}/scr++/ClassRingbuffer.o
POSSIBLE_DEPFILES=${OBJECTDIR}/src/daq_dev.o.d ${OBJECTDIR}/src/ringbuffer.o.d ${OBJECTDIR}/src/SUDD.o.d ${OBJECTDIR}/src/Timer2CTC.o.d ${OBJECTDIR}/src/usart_debug.o.d ${OBJECTDIR}/src/usart_drv.o.d ${OBJECTDIR}/src/usb_api.o.d ${OBJECTDIR}/src/usb_drv.o.d ${OBJECTDIR}/src/usb_isr.o.d ${OBJECTDIR}/src/usb_spec.o.d ${OBJECTDIR}/src/usb_ControlEndpoint.o.d ${OBJECTDIR}/scr++/ClassRingbuffer.o.d

# Object Files
OBJECTFILES=${OBJECTDIR}/src/daq_dev.o ${OBJECTDIR}/src/ringbuffer.o ${OBJECTDIR}/src/SUDD.o ${OBJECTDIR}/src/Timer2CTC.o ${OBJECTDIR}/src/usart_debug.o ${OBJECTDIR}/src/usart_drv.o ${OBJECTDIR}/src/usb_api.o ${OBJECTDIR}/src/usb_drv.o ${OBJECTDIR}/src/usb_isr.o ${OBJECTDIR}/src/usb_spec.o ${OBJECTDIR}/src/usb_ControlEndpoint.o ${OBJECTDIR}/scr++/ClassRingbuffer.o

# Source Files
SOURCEFILES=src/daq_dev.c src/ringbuffer.c src/SUDD.c src/Timer2CTC.c src/usart_debug.c src/usart_drv.c src/usb_api.c src/usb_drv.c src/usb_isr.c src/usb_spec.c src/usb_ControlEndpoint.c scr++/ClassRingbuffer.cpp

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
	${MAKE}  -f nbproject/Makefile-release_CPP.mk ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

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
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/release_CPP/90702a47987b9427101e2d1eda84910eee8bb1c5 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o  -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/release_CPP/f6bfd2a9a37b903dd7011db93b54cc34a031acfe .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o  -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/release_CPP/3de37fafe45db471701aed36139de5ec01334c45 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o  -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/release_CPP/1aa4b1f9fca6b8145c18062656725bb9580c4aad .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o  -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/release_CPP/5c9a77f2aebc78c42ea7ba4063192df5222b595c .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o  -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/release_CPP/cbc641af40e695e2536e01f6ee5dbe6be664b815 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o  -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/release_CPP/50e5ba4cc2dc6df74f976a47d698167234e4ae2f .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o  -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/release_CPP/78033a65b0dd555159f10923e08400135f698b2c .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o  -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/release_CPP/d7fb4b88c3b215aed9c94945c5f299eebbfbb4e9 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o  -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/release_CPP/8bd095fd4d3293700c2c46587f6db77856fe33 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o  -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_ControlEndpoint.o: src/usb_ControlEndpoint.c  .generated_files/flags/release_CPP/ae6cb8d54c6dc156e1d3ab833fd7d915db47e0da .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o.d 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT ${OBJECTDIR}/src/usb_ControlEndpoint.o  -o ${OBJECTDIR}/src/usb_ControlEndpoint.o src/usb_ControlEndpoint.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
else
${OBJECTDIR}/src/daq_dev.o: src/daq_dev.c  .generated_files/flags/release_CPP/12528916842b2c19a9c4707e4de2959f83de0f16 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/daq_dev.o.d 
	@${RM} ${OBJECTDIR}/src/daq_dev.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/daq_dev.o.d" -MT "${OBJECTDIR}/src/daq_dev.o.d" -MT ${OBJECTDIR}/src/daq_dev.o  -o ${OBJECTDIR}/src/daq_dev.o src/daq_dev.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/ringbuffer.o: src/ringbuffer.c  .generated_files/flags/release_CPP/e58d2a7735f7134390afeb0845286ec138093e50 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/src/ringbuffer.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/ringbuffer.o.d" -MT "${OBJECTDIR}/src/ringbuffer.o.d" -MT ${OBJECTDIR}/src/ringbuffer.o  -o ${OBJECTDIR}/src/ringbuffer.o src/ringbuffer.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/SUDD.o: src/SUDD.c  .generated_files/flags/release_CPP/88266410320f83381ca96c68dfdda7d7622ec4f5 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/src/SUDD.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/SUDD.o.d" -MT "${OBJECTDIR}/src/SUDD.o.d" -MT ${OBJECTDIR}/src/SUDD.o  -o ${OBJECTDIR}/src/SUDD.o src/SUDD.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/Timer2CTC.o: src/Timer2CTC.c  .generated_files/flags/release_CPP/94e4625747be0c9a7e52f26433d237e33fa29841 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o.d 
	@${RM} ${OBJECTDIR}/src/Timer2CTC.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/Timer2CTC.o.d" -MT "${OBJECTDIR}/src/Timer2CTC.o.d" -MT ${OBJECTDIR}/src/Timer2CTC.o  -o ${OBJECTDIR}/src/Timer2CTC.o src/Timer2CTC.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usart_debug.o: src/usart_debug.c  .generated_files/flags/release_CPP/312f3b4c86384efbf6f3280d4bd238078a8c1183 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/src/usart_debug.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usart_debug.o.d" -MT "${OBJECTDIR}/src/usart_debug.o.d" -MT ${OBJECTDIR}/src/usart_debug.o  -o ${OBJECTDIR}/src/usart_debug.o src/usart_debug.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usart_drv.o: src/usart_drv.c  .generated_files/flags/release_CPP/cb085ccb77fd34ab655ba9813eca3c7e8ed10b10 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usart_drv.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usart_drv.o.d" -MT "${OBJECTDIR}/src/usart_drv.o.d" -MT ${OBJECTDIR}/src/usart_drv.o  -o ${OBJECTDIR}/src/usart_drv.o src/usart_drv.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_api.o: src/usb_api.c  .generated_files/flags/release_CPP/a10d917db648102836f4486dee8586730af57e43 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/src/usb_api.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_api.o.d" -MT "${OBJECTDIR}/src/usb_api.o.d" -MT ${OBJECTDIR}/src/usb_api.o  -o ${OBJECTDIR}/src/usb_api.o src/usb_api.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_drv.o: src/usb_drv.c  .generated_files/flags/release_CPP/eb3d5741e202f65e8cdf5a1d62eb2276e17b2b79 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/src/usb_drv.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_drv.o.d" -MT "${OBJECTDIR}/src/usb_drv.o.d" -MT ${OBJECTDIR}/src/usb_drv.o  -o ${OBJECTDIR}/src/usb_drv.o src/usb_drv.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_isr.o: src/usb_isr.c  .generated_files/flags/release_CPP/5f4ed060f7c063ea3116a368cf4e93721b29dc2a .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/src/usb_isr.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_isr.o.d" -MT "${OBJECTDIR}/src/usb_isr.o.d" -MT ${OBJECTDIR}/src/usb_isr.o  -o ${OBJECTDIR}/src/usb_isr.o src/usb_isr.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_spec.o: src/usb_spec.c  .generated_files/flags/release_CPP/d08dcee73c036b33238a39078148ba4ac8420b6 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/src/usb_spec.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_spec.o.d" -MT "${OBJECTDIR}/src/usb_spec.o.d" -MT ${OBJECTDIR}/src/usb_spec.o  -o ${OBJECTDIR}/src/usb_spec.o src/usb_spec.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
${OBJECTDIR}/src/usb_ControlEndpoint.o: src/usb_ControlEndpoint.c  .generated_files/flags/release_CPP/a3af21052189cbba632f8bfa91b9360da22f9fc2 .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/src" 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o.d 
	@${RM} ${OBJECTDIR}/src/usb_ControlEndpoint.o 
	 ${MP_CPPC}  $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums  -I "scr++" -I "src" -Wall -MD -MP -MF "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT "${OBJECTDIR}/src/usb_ControlEndpoint.o.d" -MT ${OBJECTDIR}/src/usb_ControlEndpoint.o  -o ${OBJECTDIR}/src/usb_ControlEndpoint.o src/usb_ControlEndpoint.c  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compileCPP
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/scr++/ClassRingbuffer.o: scr++/ClassRingbuffer.cpp  .generated_files/flags/release_CPP/3e7595ba5eb188c92f644c5ce1236c1e1d8b7d2f .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/scr++" 
	@${RM} ${OBJECTDIR}/scr++/ClassRingbuffer.o.d 
	@${RM} ${OBJECTDIR}/scr++/ClassRingbuffer.o 
	 ${MP_CPPC} $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS} -g -DDEBUG  -gdwarf-2  -x c++ -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -I "src" -I "scr++" -Wall -MD -MP -MF "${OBJECTDIR}/scr++/ClassRingbuffer.o.d" -MT "${OBJECTDIR}/scr++/ClassRingbuffer.o.d" -MT ${OBJECTDIR}/scr++/ClassRingbuffer.o  -o ${OBJECTDIR}/scr++/ClassRingbuffer.o scr++/ClassRingbuffer.cpp  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
else
${OBJECTDIR}/scr++/ClassRingbuffer.o: scr++/ClassRingbuffer.cpp  .generated_files/flags/release_CPP/7c90085f6095ebfa49e6ee856f3d43e3a5fe8fae .generated_files/flags/release_CPP/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/scr++" 
	@${RM} ${OBJECTDIR}/scr++/ClassRingbuffer.o.d 
	@${RM} ${OBJECTDIR}/scr++/ClassRingbuffer.o 
	 ${MP_CPPC} $(MP_EXTRA_CC_PRE) -mmcu=at90usb1287 ${PACK_COMPILER_OPTIONS} ${PACK_COMMON_OPTIONS}  -x c++ -c -D__$(MP_PROCESSOR_OPTION)__  -funsigned-char -funsigned-bitfields -O1 -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -I "src" -I "scr++" -Wall -MD -MP -MF "${OBJECTDIR}/scr++/ClassRingbuffer.o.d" -MT "${OBJECTDIR}/scr++/ClassRingbuffer.o.d" -MT ${OBJECTDIR}/scr++/ClassRingbuffer.o  -o ${OBJECTDIR}/scr++/ClassRingbuffer.o scr++/ClassRingbuffer.cpp  -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD) 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    
	@${MKDIR} ${DISTDIR} 
	${MP_CPPC} $(MP_EXTRA_LD_PRE) -mmcu=at90usb1287 ${PACK_COMMON_OPTIONS}   -gdwarf-2 -D__$(MP_PROCESSOR_OPTION)__  -Wl,-Map="${DISTDIR}\At90UsbMPLAB.X.${IMAGE_TYPE}.map"    -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}      -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD)  -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=__ICD2RAM=1,--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1 -Wl,--gc-sections -Wl,--start-group  -Wl,-lm -Wl,--end-group 
	
	
	
	
	
	
else
${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} ${DISTDIR} 
	${MP_CPPC} $(MP_EXTRA_LD_PRE) -mmcu=at90usb1287 ${PACK_COMMON_OPTIONS}  -D__$(MP_PROCESSOR_OPTION)__  -Wl,-Map="${DISTDIR}\At90UsbMPLAB.X.${IMAGE_TYPE}.map"    -o ${DISTDIR}/At90UsbMPLAB.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}      -DXPRJ_release_CPP=$(CND_CONF)  $(COMPARISON_BUILD)  -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION) -Wl,--gc-sections -Wl,--start-group  -Wl,-lm -Wl,--end-group 
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
