# The following variables contains the files used by the different stages of the build process.
set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_assemble)
set_source_files_properties(${At90UsbMPLAB_default_default_XC8_FILE_TYPE_assemble} PROPERTIES LANGUAGE ASM)

# For assembly files, add "." to the include path for each file so that .include with a relative path works
foreach(source_file ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_assemble})
        set_source_files_properties(${source_file} PROPERTIES INCLUDE_DIRECTORIES "$<PATH:NORMAL_PATH,$<PATH:REMOVE_FILENAME,${source_file}>>")
endforeach()

set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_assembleWithPreprocess)
set_source_files_properties(${At90UsbMPLAB_default_default_XC8_FILE_TYPE_assembleWithPreprocess} PROPERTIES LANGUAGE ASM)

# For assembly files, add "." to the include path for each file so that .include with a relative path works
foreach(source_file ${At90UsbMPLAB_default_default_XC8_FILE_TYPE_assembleWithPreprocess})
        set_source_files_properties(${source_file} PROPERTIES INCLUDE_DIRECTORIES "$<PATH:NORMAL_PATH,$<PATH:REMOVE_FILENAME,${source_file}>>")
endforeach()

set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_compile
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/SUDD.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/Timer2CTC.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/daq_dev.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/ringbuffer.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/usart_debug.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/usart_drv.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/usb_api.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/usb_drv.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/usb_isr.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/usb_requests.c"
    "${CMAKE_CURRENT_SOURCE_DIR}/../../../src/usb_spec.c")
set_source_files_properties(${At90UsbMPLAB_default_default_XC8_FILE_TYPE_compile} PROPERTIES LANGUAGE C)
set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_compile_cpp)
set_source_files_properties(${At90UsbMPLAB_default_default_XC8_FILE_TYPE_compile_cpp} PROPERTIES LANGUAGE CXX)
set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_link)
set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_ihex)
set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_eep)
set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_lss)
set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_srec)
set(At90UsbMPLAB_default_default_XC8_FILE_TYPE_objcopy_sig)
set(At90UsbMPLAB_default_image_name "default.elf")
set(At90UsbMPLAB_default_image_base_name "default")

# The output directory of the final image.
set(At90UsbMPLAB_default_output_dir "${CMAKE_CURRENT_SOURCE_DIR}/../../../out/At90UsbMPLAB")

# The full path to the final image.
set(At90UsbMPLAB_default_full_path_to_image ${At90UsbMPLAB_default_output_dir}/${At90UsbMPLAB_default_image_name})

# Potential output file extensions
set(output_extensions
    .hex
    .lss
    .eep
    .srec
    .usersignatures)
list(TRANSFORM output_extensions PREPEND "${At90UsbMPLAB_default_output_dir}/${At90UsbMPLAB_default_image_base_name}")
