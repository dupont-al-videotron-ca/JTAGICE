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
FINAL_IMAGE=${DISTDIR}/Embedded.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
else
IMAGE_TYPE=production
OUTPUT_SUFFIX=hex
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=${DISTDIR}/Embedded.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
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
SOURCEFILES_QUOTED_IF_SPACED=AT90USB++/Src/SUDD.c AT90USB++/Src/usart_debug.c AT90USB++/Src/usart_drv.c AT90USB++/Src/usb_api.c AT90USB++/Src/usb_drv.c AT90USB++/Src/usb_requests.c AT90USB++/Src/usb_spec.c AT90USB++/Src++/AT90UsbDevice.cpp AT90USB++/Src++/main.cpp AT90USB++/Src++/new.cpp AT90USB++/Src++/ringbuffer.cpp AT90USB++/Src++/UsbEndpointBase.cpp AT90USB++/Src++/UsbEndpointControl.cpp AT90USB++/Src++/UsbEndpointIn.cpp AT90USB++/Src++/UsbEndpointOut.cpp AT90USB++/Src++/usb_isr.cpp

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/AT90USB++/Src/SUDD.o ${OBJECTDIR}/AT90USB++/Src/usart_debug.o ${OBJECTDIR}/AT90USB++/Src/usart_drv.o ${OBJECTDIR}/AT90USB++/Src/usb_api.o ${OBJECTDIR}/AT90USB++/Src/usb_drv.o ${OBJECTDIR}/AT90USB++/Src/usb_requests.o ${OBJECTDIR}/AT90USB++/Src/usb_spec.o ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o ${OBJECTDIR}/AT90USB++/Src++/main.o ${OBJECTDIR}/AT90USB++/Src++/new.o ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o
POSSIBLE_DEPFILES=${OBJECTDIR}/AT90USB++/Src/SUDD.o.d ${OBJECTDIR}/AT90USB++/Src/usart_debug.o.d ${OBJECTDIR}/AT90USB++/Src/usart_drv.o.d ${OBJECTDIR}/AT90USB++/Src/usb_api.o.d ${OBJECTDIR}/AT90USB++/Src/usb_drv.o.d ${OBJECTDIR}/AT90USB++/Src/usb_requests.o.d ${OBJECTDIR}/AT90USB++/Src/usb_spec.o.d ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o.d ${OBJECTDIR}/AT90USB++/Src++/main.o.d ${OBJECTDIR}/AT90USB++/Src++/new.o.d ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o.d ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o.d ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o.d ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o.d ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o.d ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o.d

# Object Files
OBJECTFILES=${OBJECTDIR}/AT90USB++/Src/SUDD.o ${OBJECTDIR}/AT90USB++/Src/usart_debug.o ${OBJECTDIR}/AT90USB++/Src/usart_drv.o ${OBJECTDIR}/AT90USB++/Src/usb_api.o ${OBJECTDIR}/AT90USB++/Src/usb_drv.o ${OBJECTDIR}/AT90USB++/Src/usb_requests.o ${OBJECTDIR}/AT90USB++/Src/usb_spec.o ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o ${OBJECTDIR}/AT90USB++/Src++/main.o ${OBJECTDIR}/AT90USB++/Src++/new.o ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o

# Source Files
SOURCEFILES=AT90USB++/Src/SUDD.c AT90USB++/Src/usart_debug.c AT90USB++/Src/usart_drv.c AT90USB++/Src/usb_api.c AT90USB++/Src/usb_drv.c AT90USB++/Src/usb_requests.c AT90USB++/Src/usb_spec.c AT90USB++/Src++/AT90UsbDevice.cpp AT90USB++/Src++/main.cpp AT90USB++/Src++/new.cpp AT90USB++/Src++/ringbuffer.cpp AT90USB++/Src++/UsbEndpointBase.cpp AT90USB++/Src++/UsbEndpointControl.cpp AT90USB++/Src++/UsbEndpointIn.cpp AT90USB++/Src++/UsbEndpointOut.cpp AT90USB++/Src++/usb_isr.cpp

# Pack Options 
PACK_COMMON_OPTIONS=-I "${CMSIS_DIR}/CMSIS/Core/Include"



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
	${MAKE}  -f nbproject/Makefile-default.mk ${DISTDIR}/Embedded.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=32WM_BZ6204
MP_LINKER_FILE_OPTION=
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
${OBJECTDIR}/AT90USB++/Src/SUDD.o: AT90USB++/Src/SUDD.c  .generated_files/flags/default/50f5c94148bb12bff500712d90fc1de5bea6200c .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/SUDD.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/SUDD.o.d" -o ${OBJECTDIR}/AT90USB++/Src/SUDD.o AT90USB++/Src/SUDD.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usart_debug.o: AT90USB++/Src/usart_debug.c  .generated_files/flags/default/9757db065ff7f4f7878118f720b2ddb4d57370e7 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usart_debug.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usart_debug.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usart_debug.o AT90USB++/Src/usart_debug.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usart_drv.o: AT90USB++/Src/usart_drv.c  .generated_files/flags/default/2046fa50ea1687be14c24b6e3c15d3d7d48917bc .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usart_drv.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usart_drv.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usart_drv.o AT90USB++/Src/usart_drv.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usb_api.o: AT90USB++/Src/usb_api.c  .generated_files/flags/default/9746098aa126967d4c8964fd579d510bf6d916d2 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_api.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usb_api.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usb_api.o AT90USB++/Src/usb_api.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usb_drv.o: AT90USB++/Src/usb_drv.c  .generated_files/flags/default/6ae8b073785af6b4a20f4469789a1b043105dcc3 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_drv.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usb_drv.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usb_drv.o AT90USB++/Src/usb_drv.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usb_requests.o: AT90USB++/Src/usb_requests.c  .generated_files/flags/default/b1a333c67fd043e3504e9ff952d300e29d65a7ba .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_requests.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_requests.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usb_requests.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usb_requests.o AT90USB++/Src/usb_requests.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usb_spec.o: AT90USB++/Src/usb_spec.c  .generated_files/flags/default/a89e3fd095ea5c27f157f0cf3de46d8c373d793e .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_spec.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usb_spec.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usb_spec.o AT90USB++/Src/usb_spec.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
else
${OBJECTDIR}/AT90USB++/Src/SUDD.o: AT90USB++/Src/SUDD.c  .generated_files/flags/default/5e937cb9313d5e1b3966fb702e4c2d1bb84df02d .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/SUDD.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/SUDD.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/SUDD.o.d" -o ${OBJECTDIR}/AT90USB++/Src/SUDD.o AT90USB++/Src/SUDD.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usart_debug.o: AT90USB++/Src/usart_debug.c  .generated_files/flags/default/f60350ddeae69d0a3370e1b00926f525c3dc04fa .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usart_debug.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usart_debug.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usart_debug.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usart_debug.o AT90USB++/Src/usart_debug.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usart_drv.o: AT90USB++/Src/usart_drv.c  .generated_files/flags/default/9bc4e20bea473052fe54fcb2570d989aae65d4b .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usart_drv.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usart_drv.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usart_drv.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usart_drv.o AT90USB++/Src/usart_drv.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usb_api.o: AT90USB++/Src/usb_api.c  .generated_files/flags/default/aa7bdcdd2cde3bc60f621f9b681877b69351c2d5 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_api.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_api.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usb_api.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usb_api.o AT90USB++/Src/usb_api.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usb_drv.o: AT90USB++/Src/usb_drv.c  .generated_files/flags/default/e26d5b3e8d4e61354155aaaea7a4106bb0220d37 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_drv.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_drv.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usb_drv.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usb_drv.o AT90USB++/Src/usb_drv.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usb_requests.o: AT90USB++/Src/usb_requests.c  .generated_files/flags/default/31ecf0597218eb648b07fa4e68fff63c125e4abe .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_requests.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_requests.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usb_requests.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usb_requests.o AT90USB++/Src/usb_requests.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src/usb_spec.o: AT90USB++/Src/usb_spec.c  .generated_files/flags/default/866065fb47c70a657c77551ed33cd92531332c39 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_spec.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src/usb_spec.o 
	${MP_CPPC}  $(MP_EXTRA_CC_PRE)  -g -x c -c -mprocessor=$(MP_PROCESSOR_OPTION)  -ffunction-sections -fdata-sections -O1 -fno-common -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src/usb_spec.o.d" -o ${OBJECTDIR}/AT90USB++/Src/usb_spec.o AT90USB++/Src/usb_spec.c    -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compileCPP
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o: AT90USB++/Src++/AT90UsbDevice.cpp  .generated_files/flags/default/175449d7e0346ca611425a983331878133b71992 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o AT90USB++/Src++/AT90UsbDevice.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/main.o: AT90USB++/Src++/main.cpp  .generated_files/flags/default/422f502efdcb7e245df5619951df457125bfff8b .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/main.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/main.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/main.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/main.o AT90USB++/Src++/main.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/new.o: AT90USB++/Src++/new.cpp  .generated_files/flags/default/4274dc65fcec531a1cd500fcc450540d30f85f83 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/new.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/new.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/new.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/new.o AT90USB++/Src++/new.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o: AT90USB++/Src++/ringbuffer.cpp  .generated_files/flags/default/d893a47c7aa9672f4afc40e055db91625d589918 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o AT90USB++/Src++/ringbuffer.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o: AT90USB++/Src++/UsbEndpointBase.cpp  .generated_files/flags/default/8e32d80992d0cf642666f1e50521a59cf2cf51e3 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o AT90USB++/Src++/UsbEndpointBase.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o: AT90USB++/Src++/UsbEndpointControl.cpp  .generated_files/flags/default/b6e5f125590573dc9781aa9d03d745b2e8064a71 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o AT90USB++/Src++/UsbEndpointControl.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o: AT90USB++/Src++/UsbEndpointIn.cpp  .generated_files/flags/default/82cc269ca58c794142c4816c91782e7bf65a3486 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o AT90USB++/Src++/UsbEndpointIn.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o: AT90USB++/Src++/UsbEndpointOut.cpp  .generated_files/flags/default/3cd01c8bd6de0be9c32370f4d73d39a693b281e3 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o AT90USB++/Src++/UsbEndpointOut.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/usb_isr.o: AT90USB++/Src++/usb_isr.cpp  .generated_files/flags/default/c3b99f163d1768b74732a730417ba33ec1ebccb5 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG   -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/usb_isr.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o AT90USB++/Src++/usb_isr.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
else
${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o: AT90USB++/Src++/AT90UsbDevice.cpp  .generated_files/flags/default/df54b89c633d515760353d678fd039ba0cece72c .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/AT90UsbDevice.o AT90USB++/Src++/AT90UsbDevice.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/main.o: AT90USB++/Src++/main.cpp  .generated_files/flags/default/123d32100975427b1a2412a6c1fdc3f60ae72500 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/main.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/main.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/main.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/main.o AT90USB++/Src++/main.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/new.o: AT90USB++/Src++/new.cpp  .generated_files/flags/default/5a416662275b7aca3f9b13f35e39a9dd9ad4ddf7 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/new.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/new.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/new.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/new.o AT90USB++/Src++/new.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o: AT90USB++/Src++/ringbuffer.cpp  .generated_files/flags/default/893318e473f09077358f10b7db7468db0a3297a3 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/ringbuffer.o AT90USB++/Src++/ringbuffer.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o: AT90USB++/Src++/UsbEndpointBase.cpp  .generated_files/flags/default/f6ac9971da8ea33ac064d353b81fb054fd03c700 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointBase.o AT90USB++/Src++/UsbEndpointBase.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o: AT90USB++/Src++/UsbEndpointControl.cpp  .generated_files/flags/default/18524695884c7c68c5c508c9ee7859d7844a1711 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointControl.o AT90USB++/Src++/UsbEndpointControl.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o: AT90USB++/Src++/UsbEndpointIn.cpp  .generated_files/flags/default/344c886bf85fef8919a66e645c2e7bbc140a14e7 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointIn.o AT90USB++/Src++/UsbEndpointIn.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o: AT90USB++/Src++/UsbEndpointOut.cpp  .generated_files/flags/default/5fabf8fe473d041e5d23cb270c90090a48263f66 .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/UsbEndpointOut.o AT90USB++/Src++/UsbEndpointOut.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
${OBJECTDIR}/AT90USB++/Src++/usb_isr.o: AT90USB++/Src++/usb_isr.cpp  .generated_files/flags/default/fb38c3ab66eeb592330c0d9cc4791eaaa4892f5f .generated_files/flags/default/da39a3ee5e6b4b0d3255bfef95601890afd80709
	@${MKDIR} "${OBJECTDIR}/AT90USB++/Src++" 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o.d 
	@${RM} ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o 
	${MP_CPPC} $(MP_EXTRA_CC_PRE)  -g -x c++ -c -mprocessor=$(MP_PROCESSOR_OPTION)  -frtti -fexceptions -fno-check-new -fenforce-eh-specs -ffunction-sections -O1 -MP -MMD -MF "${OBJECTDIR}/AT90USB++/Src++/usb_isr.o.d" -o ${OBJECTDIR}/AT90USB++/Src++/usb_isr.o AT90USB++/Src++/usb_isr.cpp   -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -mdfp="${DFP_DIR}/PIC32WM_BZ6024" ${PACK_COMMON_OPTIONS} 
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${DISTDIR}/Embedded.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    
	@${MKDIR} ${DISTDIR} 
	${MP_CPPC} $(MP_EXTRA_LD_PRE) -g   -mprocessor=$(MP_PROCESSOR_OPTION)  -o ${DISTDIR}/Embedded.${IMAGE_TYPE}.${OUTPUT_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}          -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=__ICD2RAM=1,--defsym=__MPLAB_DEBUG=1,--defsym=__DEBUG=1,-D=__DEBUG_D,--defsym=_min_heap_size=0,--gc-sections,-Map="${DISTDIR}/${PROJECTNAME}.${IMAGE_TYPE}.map",--memorysummary,${DISTDIR}/memoryfile.xml -mdfp="${DFP_DIR}/PIC32WM_BZ6024"
	
else
${DISTDIR}/Embedded.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} ${DISTDIR} 
	${MP_CPPC} $(MP_EXTRA_LD_PRE)  -mprocessor=$(MP_PROCESSOR_OPTION)  -o ${DISTDIR}/Embedded.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX} ${OBJECTFILES_QUOTED_IF_SPACED}          -DXPRJ_default=$(CND_CONF)  $(COMPARISON_BUILD)  -Wl,--defsym=__MPLAB_BUILD=1$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=_min_heap_size=0,--gc-sections,-Map="${DISTDIR}/${PROJECTNAME}.${IMAGE_TYPE}.map",--memorysummary,${DISTDIR}/memoryfile.xml -mdfp="${DFP_DIR}/PIC32WM_BZ6024"
	${MP_CC_DIR}\\xc32-bin2hex ${DISTDIR}/Embedded.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX} 
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
