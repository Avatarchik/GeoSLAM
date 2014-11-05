#!/usr/bin/env python
# encoding: utf-8


"""
Using mod-pbxproj:
https://bitbucket.org/icalderon/mod-pbxproj
"""

import sys
from mod_pbxproj import XcodeProject


installPath = sys.argv[1]
pbx = installPath + '/Unity-iPhone.xcodeproj/project.pbxproj'

project = XcodeProject.Load(pbx)
if project is None:
	print >>sys.stderr, "failed to open Xcode project at %s" % pbx
	exit(-1)

project.add_file_if_doesnt_exist('System/Library/Frameworks/Accelerate.framework', tree='SDKROOT')
project.add_file_if_doesnt_exist('usr/lib/libc++.dylib', tree='SDKROOT')
project.add_file_if_doesnt_exist('Libraries/Shader.vsh', tree='SOURCE_ROOT')
project.add_file_if_doesnt_exist('Libraries/Shader.fsh', tree='SOURCE_ROOT')

if project.modified:
	project.backup()
	project.saveFormat3_2()
