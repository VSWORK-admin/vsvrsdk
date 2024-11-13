# FFmpeg for Unity

## 概要

これはUnityのエディタ上やアプリ上でFFmpeg(http://ffmpeg.org/)を使用できるアセットです。

以下のようなことが出来ます。

- 動画の再生(mp4, avi, mov etc.)
- ゲーム画面、もしくはゲーム内カメラからの録画
- 動画の変換
- Youtube(https://www.youtube.com/)へのライブ配信(rtmp etc.)
など

## 対応環境

- Unityエディタ(Windows/Mac/Linux)
- スタンドアローン(Windows/Mac/Linux)(Mono/IL2CPP)
- Android
- iOS

## FFmpeg本体について

このアセットでは以下のスクリプトからビルドしたFFmpeg本体(Windows/Mac/Linux)、もしくはプラグイン(Windows/Android/iOS)を使用しております。

外部ライブラリは一部のみ（「ライセンス」を参照）にしておりますので、使用したい機能がない場合は手順に従って再ビルドし、それに置き換えてください。

- Windows(https://github.com/NON906/ffmpeg-windows-build-helpers)
- Mac/Linux(https://github.com/NON906/ffmpeg-build-script)
- Android/iOS(https://github.com/NON906/ffmpeg-kit)

## ライセンス

このアセットはAsset Store EULAによって管理されていますが、次のコンポーネントは以下に示すライセンスによって管理されています:

ffmpeg: LGPLv2.1
Copyright (c) 2000-2021 the FFmpeg developers

ffmpeg-kit: LGPLv3 (for Android/iOS/Mac)

SmartException: BSD 3-Clause (for Android)
Copyright (c) 2019-2020, Taner Sener

openh264: BSD 2-Clause
Copyright (c) 2013, Cisco Systems

libvpx: BSD 3-Clause
Copyright (c) 2010, The WebM Project authors. All rights reserved.

aom: BSD 2-Clause
Copyright (c) 2016, Alliance for Open Media. All rights reserved.

opus: BSD 3-Clause
Copyright 2001-2011 Xiph.Org, Skype Limited, Octasic,
                    Jean-Marc Valin, Timothy B. Terriberry,
                    CSIRO, Gregory Maxwell, Mark Borgerding,
                    Erik de Castro Lopo

vorbis: BSD 3-Clause
Copyright (c) 2002-2020 Xiph.org Foundation

theora: BSD 3-Clause
Copyright (C) 2002-2009 Xiph.org Foundation

libwebp: BSD 3-Clause
Copyright (c) 2010, Google Inc. All rights reserved.

giflib: MIT License
The GIFLIB distribution is Copyright (c) 1997  Eric S. Raymond

libjpeg_turbo: BSD 3-Clause
Copyright (C)2009-2020 D. R. Commander.  All Rights Reserved.
Copyright (C)2015 Viktor Szathmáry.  All Rights Reserved.

libogg: BSD 3-Clause
Copyright (c) 2002, Xiph.org Foundation

libpng: PNG Reference Library License version 2
 * Copyright (c) 1995-2019 The PNG Reference Library Authors.
 * Copyright (c) 2018-2019 Cosmin Truta.
 * Copyright (c) 2000-2002, 2004, 2006-2018 Glenn Randers-Pehrson.
 * Copyright (c) 1996-1997 Andreas Dilger.
 * Copyright (c) 1995-1996 Guy Eric Schalnat, Group 42, Inc.

libtiff: libtiff License
Copyright (c) 1988-1997 Sam Leffler
Copyright (c) 1991-1997 Silicon Graphics, Inc.

gmp: LGPLv3
Copyright 1991, 1996, 1999, 2000, 2007 Free Software Foundation, Inc.

nettle: LGPLv3
Copyright (C) 2007 Free Software Foundation, Inc. <http://fsf.org/>

gnutls: LGPLv2.1
Copyright (C) 1991, 1999 Free Software Foundation, Inc.

winpthreads: MIT License & BSD 3-Clause
Copyright (c) 2011 mingw-w64 project (MIT)
 * (C) 2010 Lockless Inc.
 * All rights reserved. (BSD)

lame: LGPLv2
 Copyright (C) 1991 Free Software Foundation, Inc.
    		    59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
(www.mp3dev.org)

sdl: zlib License
Copyright (C) 1997-2022 Sam Lantinga <slouken@libsdl.org>