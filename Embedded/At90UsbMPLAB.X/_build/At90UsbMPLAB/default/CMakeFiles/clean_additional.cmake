# Additional clean files
cmake_minimum_required(VERSION 3.16)

if("${CONFIG}" STREQUAL "" OR "${CONFIG}" STREQUAL "")
  file(REMOVE_RECURSE
  "D:\\ALAIN\\AT90USB\\Projects\\AT90USB\\At90UsbMPLAB.X\\out\\At90UsbMPLAB\\default.eep"
  "D:\\ALAIN\\AT90USB\\Projects\\AT90USB\\At90UsbMPLAB.X\\out\\At90UsbMPLAB\\default.hex"
  "D:\\ALAIN\\AT90USB\\Projects\\AT90USB\\At90UsbMPLAB.X\\out\\At90UsbMPLAB\\default.lss"
  "D:\\ALAIN\\AT90USB\\Projects\\AT90USB\\At90UsbMPLAB.X\\out\\At90UsbMPLAB\\default.srec"
  "D:\\ALAIN\\AT90USB\\Projects\\AT90USB\\At90UsbMPLAB.X\\out\\At90UsbMPLAB\\default.usersignatures"
  )
endif()
