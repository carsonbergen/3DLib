#!/usr/bin/env python
import os
import sys

env = SConscript("godot-cpp/SConstruct")

# For reference:
# - CCFLAGS are compilation flags shared between C and C++
# - CFLAGS are for C-specific compilation flags
# - CXXFLAGS are for C++-specific compilation flags
# - CPPFLAGS are for pre-processor flags
# - CPPDEFINES are for pre-processor defines
# - LINKFLAGS are for linking flags

# tweak this if you want to use different folders, or more folders, to store your source code in.
env.Append(CPPPATH=["src/", "src/Cameras"])
sources = Glob("src/*.cpp")


def getSubdirs(absPathDir) :  
    lst = [ name for name in os.listdir(absPathDir) if os.path.isdir(os.path.join(absPathDir, name)) and name[0] != '.' ]
    lst.sort()
    return lst

path = "./src"
modules = getSubdirs(path)

for module in modules:
    sources += Glob(os.path.join(path, module, '*.cpp'))

print("\nSources")
for s in sources:
    print("\t%s"%s)
print()

if env["platform"] == "macos":
    library = env.SharedLibrary(
        "demo/bin/3dlib.{}.{}.framework/3dlib.{}.{}".format(
            env["platform"], env["target"], env["platform"], env["target"]
        ),
        source=sources,
    )
else:
    library = env.SharedLibrary(
        "demo/bin/3dlib{}{}".format(env["suffix"], env["SHLIBSUFFIX"]),
        source=sources,
    )

Default(library)
