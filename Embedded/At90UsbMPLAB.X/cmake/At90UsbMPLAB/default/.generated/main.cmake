include("${CMAKE_CURRENT_LIST_DIR}/rule.cmake")
include("${CMAKE_CURRENT_LIST_DIR}/file.cmake")

set(At90UsbMPLAB_default_library_list )

# Handle files with suffix s, for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_assemble)
add_library(At90UsbMPLAB_default_default_XC8_assemble OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_assemble})
    At90UsbMPLAB_default_default_XC8_assemble_rule(At90UsbMPLAB_default_default_XC8_assemble)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_assemble>")

endif()

# Handle files with suffix S, for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_assembleWithPreprocess)
add_library(At90UsbMPLAB_default_default_XC8_assembleWithPreprocess OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_assembleWithPreprocess})
    At90UsbMPLAB_default_default_XC8_assembleWithPreprocess_rule(At90UsbMPLAB_default_default_XC8_assembleWithPreprocess)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_assembleWithPreprocess>")

endif()

# Handle files with suffix [cC], for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_compile)
add_library(At90UsbMPLAB_default_default_XC8_compile OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_compile})
    At90UsbMPLAB_default_default_XC8_compile_rule(At90UsbMPLAB_default_default_XC8_compile)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_compile>")

endif()

# Handle files with suffix cpp, for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_compile_cpp)
add_library(At90UsbMPLAB_default_default_XC8_compile_cpp OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_compile_cpp})
    At90UsbMPLAB_default_default_XC8_compile_cpp_rule(At90UsbMPLAB_default_default_XC8_compile_cpp)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_compile_cpp>")

endif()

# Handle files with suffix elf, for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_ihex)
add_library(At90UsbMPLAB_default_default_XC8_objcopy_ihex OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_ihex})
    At90UsbMPLAB_default_default_XC8_objcopy_ihex_rule(At90UsbMPLAB_default_default_XC8_objcopy_ihex)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_objcopy_ihex>")

endif()

# Handle files with suffix elf, for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_eep)
add_library(At90UsbMPLAB_default_default_XC8_objcopy_eep OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_eep})
    At90UsbMPLAB_default_default_XC8_objcopy_eep_rule(At90UsbMPLAB_default_default_XC8_objcopy_eep)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_objcopy_eep>")

endif()

# Handle files with suffix elf, for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_lss)
add_library(At90UsbMPLAB_default_default_XC8_objcopy_lss OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_lss})
    At90UsbMPLAB_default_default_XC8_objcopy_lss_rule(At90UsbMPLAB_default_default_XC8_objcopy_lss)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_objcopy_lss>")

endif()

# Handle files with suffix elf, for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_srec)
add_library(At90UsbMPLAB_default_default_XC8_objcopy_srec OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_srec})
    At90UsbMPLAB_default_default_XC8_objcopy_srec_rule(At90UsbMPLAB_default_default_XC8_objcopy_srec)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_objcopy_srec>")

endif()

# Handle files with suffix elf, for group default-XC8
if(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_sig)
add_library(At90UsbMPLAB_default_default_XC8_objcopy_sig OBJECT ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_sig})
    At90UsbMPLAB_default_default_XC8_objcopy_sig_rule(At90UsbMPLAB_default_default_XC8_objcopy_sig)
    list(APPEND At90UsbMPLAB_default_library_list "$<TARGET_OBJECTS:At90UsbMPLAB_default_default_XC8_objcopy_sig>")

endif()


# Main target for this project
add_executable(At90UsbMPLAB_default_image_CtdRLKcd ${At90UsbMPLAB_default_library_list})

set_target_properties(At90UsbMPLAB_default_image_CtdRLKcd PROPERTIES
    OUTPUT_NAME "default"
    SUFFIX ".elf"
    ADDITIONAL_CLEAN_FILES "${output_extensions}"
    RUNTIME_OUTPUT_DIRECTORY "${At90UsbMPLAB_default_output_dir}")
target_link_libraries(At90UsbMPLAB_default_image_CtdRLKcd PRIVATE ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_link})

#Add objcopy steps
At90UsbMPLAB_default_objcopy_ihex_rule(At90UsbMPLAB_default_image_CtdRLKcd)
At90UsbMPLAB_default_objcopy_eep_rule(At90UsbMPLAB_default_image_CtdRLKcd)
At90UsbMPLAB_default_objcopy_lss_rule(At90UsbMPLAB_default_image_CtdRLKcd)
At90UsbMPLAB_default_objcopy_srec_rule(At90UsbMPLAB_default_image_CtdRLKcd)
At90UsbMPLAB_default_objcopy_sig_rule(At90UsbMPLAB_default_image_CtdRLKcd)
# Add the link options from the rule file.
At90UsbMPLAB_default_link_rule( At90UsbMPLAB_default_image_CtdRLKcd)


