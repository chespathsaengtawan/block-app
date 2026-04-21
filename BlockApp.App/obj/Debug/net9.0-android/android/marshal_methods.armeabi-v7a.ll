; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [318 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [954 x i32] [
	i32 u0x0027eb9e, ; 0: System.Net.NetworkInformation.dll => 69
	i32 u0x00345a11, ; 1: lib_System.Net.Requests.dll.so => 73
	i32 u0x009b21bb, ; 2: System.Net.NameResolution.dll => 68
	i32 u0x00c8cc5d, ; 3: lib_Xamarin.AndroidX.Loader.dll.so => 239
	i32 u0x00e0bbf7, ; 4: lib_System.Xml.XmlSerializer.dll.so => 163
	i32 u0x00efe298, ; 5: System.Runtime.Intrinsics.dll => 109
	i32 u0x0119bc86, ; 6: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 178
	i32 u0x01f2c4e1, ; 7: Xamarin.AndroidX.Lifecycle.Runtime => 231
	i32 u0x0211b5dc, ; 8: Xamarin.Google.Guava.ListenableFuture.dll => 267
	i32 u0x02139ac3, ; 9: System.IO.FileSystem.DriveInfo => 48
	i32 u0x025a8054, ; 10: System.Net.WebSockets.dll => 81
	i32 u0x02664405, ; 11: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 307
	i32 u0x028aa24d, ; 12: System.Threading.Thread => 146
	i32 u0x02bda0f5, ; 13: Xamarin.KotlinX.AtomicFU.Jvm => 272
	i32 u0x03358480, ; 14: lib_Microsoft.Maui.dll.so => 186
	i32 u0x0335cdbc, ; 15: ca/Microsoft.Maui.Controls.resources => 279
	i32 u0x03f75868, ; 16: System.Diagnostics.StackTrace => 30
	i32 u0x0410f24b, ; 17: System.Security.Cryptography.Primitives => 125
	i32 u0x044bb714, ; 18: Microsoft.Maui.Graphics.dll => 188
	i32 u0x04e7b0a1, ; 19: System.Runtime.CompilerServices.VisualC.dll => 103
	i32 u0x056606a6, ; 20: lib_System.Collections.NonGeneric.dll.so => 10
	i32 u0x060d4943, ; 21: Xamarin.AndroidX.SlidingPaneLayout => 250
	i32 u0x065dd880, ; 22: lib_System.Linq.Queryable.dll.so => 61
	i32 u0x06c2cd46, ; 23: zh-HK/Microsoft.Maui.Controls.resources => 309
	i32 u0x06e4e181, ; 24: lib_Xamarin.Google.Guava.ListenableFuture.dll.so => 267
	i32 u0x06ee56d3, ; 25: lib_System.Net.Mail.dll.so => 67
	i32 u0x06ffddbc, ; 26: System.Runtime.InteropServices => 108
	i32 u0x072f9521, ; 27: Xamarin.AndroidX.SlidingPaneLayout.dll => 250
	i32 u0x074aea82, ; 28: System.Threading.Channels.dll => 140
	i32 u0x0772c6a7, ; 29: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 31
	i32 u0x0881c32f, ; 30: System.Net.WebHeaderCollection => 78
	i32 u0x08f064cf, ; 31: System.Security.Cryptography.Primitives.dll => 125
	i32 u0x097ed3c0, ; 32: System.ComponentModel.Annotations => 13
	i32 u0x098905a2, ; 33: lib_Xamarin.AndroidX.Concurrent.Futures.dll.so => 207
	i32 u0x09d975c3, ; 34: Xamarin.AndroidX.Collection.dll => 204
	i32 u0x09e60a6e, ; 35: Xamarin.KotlinX.AtomicFU.dll => 271
	i32 u0x0a0c2bd0, ; 36: lib_Xamarin.AndroidX.Activity.dll.so => 193
	i32 u0x0a81994f, ; 37: System.ServiceProcess => 133
	i32 u0x0ade3a75, ; 38: Xamarin.AndroidX.SwipeRefreshLayout.dll => 252
	i32 u0x0ae43932, ; 39: lib_Xamarin.AndroidX.Tracing.Tracing.dll.so => 253
	i32 u0x0aee6a3d, ; 40: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 308
	i32 u0x0aeedc53, ; 41: lib_Xamarin.Google.Android.Material.dll.so => 262
	i32 u0x0afca281, ; 42: System.ValueTuple.dll => 152
	i32 u0x0b0de1c3, ; 43: lib_System.Xml.XPath.XDocument.dll.so => 160
	i32 u0x0b63b1e1, ; 44: lib_System.Net.Http.Json.dll.so => 64
	i32 u0x0b721a36, ; 45: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 298
	i32 u0x0ba65f85, ; 46: vi/Microsoft.Maui.Controls.resources.dll => 308
	i32 u0x0ba8e231, ; 47: lib_System.Net.ServicePoint.dll.so => 75
	i32 u0x0be195c3, ; 48: zh-HK/Microsoft.Maui.Controls.resources.dll => 309
	i32 u0x0c38ff48, ; 49: System.ComponentModel => 18
	i32 u0x0c5df1c2, ; 50: lib_Microsoft.VisualStudio.DesignTools.XamlTapContract.dll.so => 316
	i32 u0x0c7b2e71, ; 51: Xamarin.AndroidX.Browser.dll => 202
	i32 u0x0cfa66a6, ; 52: lib_System.IO.Compression.FileSystem.dll.so => 44
	i32 u0x0d1f8edb, ; 53: System.Diagnostics.Debug => 26
	i32 u0x0d73bff4, ; 54: lib_Microsoft.Extensions.Logging.Debug.dll.so => 181
	i32 u0x0dc10265, ; 55: Microsoft.CSharp.dll => 1
	i32 u0x0dc2edec, ; 56: lib_Xamarin.AndroidX.Core.ViewTree.dll.so => 213
	i32 u0x0dc2f416, ; 57: lib_Xamarin.AndroidX.CustomView.dll.so => 215
	i32 u0x0dcb05c4, ; 58: System.Linq.Parallel => 60
	i32 u0x0dd133ce, ; 59: System.Globalization => 42
	i32 u0x0e762ada, ; 60: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 296
	i32 u0x0e794502, ; 61: BlockApp.Shared => 312
	i32 u0x0eb2f8c5, ; 62: System.Reflection.Emit.Lightweight => 92
	i32 u0x0ec71be0, ; 63: lib_System.Security.SecureString.dll.so => 130
	i32 u0x0ecfdca9, ; 64: lib_Xamarin.Android.Glide.dll.so => 189
	i32 u0x0f99119d, ; 65: Xamarin.AndroidX.ConstraintLayout.dll => 208
	i32 u0x107abf20, ; 66: System.Threading.Timer.dll => 148
	i32 u0x109c6ab8, ; 67: Xamarin.AndroidX.Lifecycle.LiveData.dll => 227
	i32 u0x10b7d2b7, ; 68: Xamarin.AndroidX.Interpolator => 224
	i32 u0x10bf9929, ; 69: cs/Microsoft.Maui.Controls.resources.dll => 280
	i32 u0x10c1d9f6, ; 70: lib_System.Data.DataSetExtensions.dll.so => 23
	i32 u0x113d3381, ; 71: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 303
	i32 u0x1159791e, ; 72: System.IO.Pipes.AccessControl.dll => 55
	i32 u0x11d123fd, ; 73: System.Net.Ping.dll => 70
	i32 u0x13031348, ; 74: Xamarin.AndroidX.Activity.dll => 193
	i32 u0x132b30dd, ; 75: System.Numerics => 84
	i32 u0x1331a702, ; 76: lib_Xamarin.Google.Crypto.Tink.Android.dll.so => 264
	i32 u0x136bf828, ; 77: lib_System.Runtime.dll.so => 117
	i32 u0x14095832, ; 78: ja/Microsoft.Maui.Controls.resources.dll => 293
	i32 u0x145ea6e0, ; 79: lib_BlockApp.Shared.dll.so => 312
	i32 u0x146817a2, ; 80: Xamarin.AndroidX.Lifecycle.Common => 225
	i32 u0x14eaf2a7, ; 81: lib_System.ComponentModel.Annotations.dll.so => 13
	i32 u0x153e1455, ; 82: it/Microsoft.Maui.Controls.resources.dll => 292
	i32 u0x15502fa0, ; 83: cs/Microsoft.Maui.Controls.resources => 280
	i32 u0x15766b7b, ; 84: System.ServiceModel.Web => 132
	i32 u0x15c177ae, ; 85: lib_Microsoft.Extensions.Configuration.dll.so => 175
	i32 u0x15e184df, ; 86: lib_System.Runtime.Loader.dll.so => 110
	i32 u0x15ebe147, ; 87: System.IO.Pipes => 56
	i32 u0x16101ba2, ; 88: lib_Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll.so => 314
	i32 u0x1658bf94, ; 89: System.Transactions.Local => 150
	i32 u0x16646418, ; 90: System.Net.ServicePoint.dll => 75
	i32 u0x16a510e1, ; 91: System.Threading.Thread.dll => 146
	i32 u0x16fe439a, ; 92: System.Memory.dll => 63
	i32 u0x1766c1f7, ; 93: System.Threading.ThreadPool.dll => 147
	i32 u0x1778984a, ; 94: lib_Xamarin.AndroidX.ResourceInspection.Annotation.dll.so => 246
	i32 u0x17969339, ; 95: _Microsoft.Android.Resource.Designer => 317
	i32 u0x180c08d0, ; 96: WindowsBase => 166
	i32 u0x195d1904, ; 97: Xamarin.AndroidX.Lifecycle.Runtime.Android => 232
	i32 u0x198cd3eb, ; 98: lib_System.Security.Cryptography.Encoding.dll.so => 123
	i32 u0x19f6996b, ; 99: sv/Microsoft.Maui.Controls.resources.dll => 304
	i32 u0x1a4e3ec4, ; 100: Xamarin.AndroidX.ConstraintLayout.Core => 209
	i32 u0x1a61054f, ; 101: System.Collections => 12
	i32 u0x1ae0ec2c, ; 102: Xamarin.AndroidX.Fragment.dll => 222
	i32 u0x1ae969b2, ; 103: System.Security.Cryptography.X509Certificates => 126
	i32 u0x1b317bfd, ; 104: System.Web.HttpUtility.dll => 153
	i32 u0x1b46a9fd, ; 105: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll.so => 233
	i32 u0x1b5932ea, ; 106: lib_Mono.Android.Runtime.dll.so => 171
	i32 u0x1b611806, ; 107: System.Runtime.Serialization.Primitives.dll => 114
	i32 u0x1bc4415d, ; 108: mscorlib => 167
	i32 u0x1bc6ffe7, ; 109: lib_Java.Interop.dll.so => 169
	i32 u0x1bff388e, ; 110: System.dll => 165
	i32 u0x1c690cb9, ; 111: Xamarin.AndroidX.Interpolator.dll => 224
	i32 u0x1c78d08a, ; 112: lib_System.Private.Uri.dll.so => 87
	i32 u0x1d48410e, ; 113: lib_Xamarin.AndroidX.SlidingPaneLayout.dll.so => 250
	i32 u0x1d4d8185, ; 114: lib_System.Runtime.Serialization.dll.so => 116
	i32 u0x1dbae811, ; 115: System.ObjectModel => 85
	i32 u0x1dd2dc50, ; 116: id/Microsoft.Maui.Controls.resources.dll => 291
	i32 u0x1e092f31, ; 117: fi/Microsoft.Maui.Controls.resources.dll => 285
	i32 u0x1e9789de, ; 118: Microsoft.Extensions.Primitives.dll => 183
	i32 u0x1f1dceb7, ; 119: lib_System.Security.Cryptography.Primitives.dll.so => 125
	i32 u0x1f443e2d, ; 120: lib_System.AppContext.dll.so => 6
	i32 u0x1f6088c2, ; 121: System.Transactions.dll => 151
	i32 u0x1f6bf43d, ; 122: hi/Microsoft.Maui.Controls.resources => 288
	i32 u0x1f9b4faa, ; 123: System.Linq.Queryable => 61
	i32 u0x20216150, ; 124: Microsoft.Extensions.Logging => 179
	i32 u0x20303736, ; 125: System.IO.FileSystem.dll => 51
	i32 u0x2080b118, ; 126: System.Runtime.Extensions => 104
	i32 u0x20924146, ; 127: System.Runtime.Serialization.Xml => 115
	i32 u0x20bbb280, ; 128: System.Globalization.Calendars => 40
	i32 u0x20f5e575, ; 129: BlockApp.Shared.dll => 312
	i32 u0x2116ab2f, ; 130: Xamarin.JSpecify.dll => 269
	i32 u0x213954e7, ; 131: Jsr305Binding => 263
	i32 u0x218bdf07, ; 132: Xamarin.AndroidX.Core.ViewTree.dll => 213
	i32 u0x21f36ef8, ; 133: Xamarin.AndroidX.Window.Extensions.Core.Core => 261
	i32 u0x22697083, ; 134: System.Security.Cryptography.Cng => 121
	i32 u0x234b6fb2, ; 135: pt-BR/Microsoft.Maui.Controls.resources.dll => 299
	i32 u0x236793de, ; 136: lib_GoogleGson.dll.so => 174
	i32 u0x2386616a, ; 137: lib_System.ServiceModel.Web.dll.so => 132
	i32 u0x2397454a, ; 138: lib_System.Collections.Specialized.dll.so => 11
	i32 u0x23d83352, ; 139: System.IO.IsolatedStorage.dll => 52
	i32 u0x23eaab34, ; 140: lib_System.Core.dll.so => 21
	i32 u0x24154ecb, ; 141: System.IO.Compression.FileSystem => 44
	i32 u0x2459aaf0, ; 142: lib_System.Net.Sockets.dll.so => 76
	i32 u0x2493d7b9, ; 143: System.Security.Cryptography.Algorithms => 120
	i32 u0x2512d1c5, ; 144: Xamarin.AndroidX.Lifecycle.Runtime.Android.dll => 232
	i32 u0x2568904f, ; 145: Xamarin.AndroidX.CustomView => 215
	i32 u0x26233b86, ; 146: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 220
	i32 u0x26249f17, ; 147: lib_Xamarin.AndroidX.CustomView.PoolingContainer.dll.so => 216
	i32 u0x262968a7, ; 148: lib_System.Reflection.Extensions.dll.so => 94
	i32 u0x262d781c, ; 149: lib-de-Microsoft.Maui.Controls.resources.dll.so => 282
	i32 u0x2660a755, ; 150: System.Net => 82
	i32 u0x27787397, ; 151: System.Text.Encodings.Web.dll => 137
	i32 u0x278c7790, ; 152: Xamarin.AndroidX.VersionedParcelable => 257
	i32 u0x27b53050, ; 153: lib_System.Data.Common.dll.so => 22
	i32 u0x27b6d01f, ; 154: Xamarin.AndroidX.Arch.Core.Common.dll => 200
	i32 u0x2814a96c, ; 155: System.Collections.Concurrent => 8
	i32 u0x282acf5e, ; 156: lib_System.IO.FileSystem.dll.so => 51
	i32 u0x28607aa1, ; 157: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 299
	i32 u0x287c1a88, ; 158: Xamarin.KotlinX.AtomicFU => 271
	i32 u0x28bdabca, ; 159: System.Net.Security => 74
	i32 u0x2904cf94, ; 160: ca/Microsoft.Maui.Controls.resources.dll => 279
	i32 u0x29293ff5, ; 161: System.Xml.Linq.dll => 156
	i32 u0x29352520, ; 162: Xamarin.KotlinX.Coroutines.Android.dll => 273
	i32 u0x29423679, ; 163: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 214
	i32 u0x295a9e3d, ; 164: System.Windows => 155
	i32 u0x296c7566, ; 165: lib_System.Xml.dll.so => 164
	i32 u0x29af2b3b, ; 166: System.Reflection.Emit => 93
	i32 u0x29bd7e5b, ; 167: Xamarin.Jetbrains.Annotations => 268
	i32 u0x29be9df3, ; 168: System.IO.Compression.ZipFile => 45
	i32 u0x2a1e8ecb, ; 169: ko/Microsoft.Maui.Controls.resources.dll => 294
	i32 u0x2a4afd4a, ; 170: de/Microsoft.Maui.Controls.resources.dll => 282
	i32 u0x2aaa494f, ; 171: Xamarin.Google.ErrorProne.TypeAnnotations => 266
	i32 u0x2b15ed29, ; 172: System.Runtime.Loader.dll => 110
	i32 u0x2ba1ca8c, ; 173: lib_System.Security.dll.so => 131
	i32 u0x2bd14e96, ; 174: System.Security.SecureString.dll => 130
	i32 u0x2cd6293c, ; 175: System.Diagnostics.Contracts.dll => 25
	i32 u0x2d052d0c, ; 176: Xamarin.Android.Glide.Annotations.dll => 190
	i32 u0x2d322560, ; 177: lib_System.Xml.XmlDocument.dll.so => 162
	i32 u0x2d445acd, ; 178: System.Net.Requests => 73
	i32 u0x2d745423, ; 179: System.IO.Pipes.dll => 56
	i32 u0x2e394f87, ; 180: System.IO.Compression => 46
	i32 u0x2eec5558, ; 181: lib_System.Reflection.dll.so => 98
	i32 u0x2f0980eb, ; 182: Microsoft.Extensions.Options => 182
	i32 u0x2f0fe5eb, ; 183: lib_System.Reflection.DispatchProxy.dll.so => 90
	i32 u0x2f1c1e69, ; 184: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 216
	i32 u0x2ff6fb9f, ; 185: System.Data.Common => 22
	i32 u0x302809e9, ; 186: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 229
	i32 u0x30a0e95c, ; 187: lib_System.Threading.Thread.dll.so => 146
	i32 u0x311247b5, ; 188: System.Private.Uri.dll => 87
	i32 u0x317d5b75, ; 189: System.IO.Compression.Brotli => 43
	i32 u0x31a103c6, ; 190: System.Xml.XPath.dll => 161
	i32 u0x31b69d60, ; 191: System.Net.Quic => 72
	i32 u0x3312831d, ; 192: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 217
	i32 u0x33e88be1, ; 193: ar/Microsoft.Maui.Controls.resources => 278
	i32 u0x340ac0b8, ; 194: Microsoft.VisualBasic => 3
	i32 u0x34505120, ; 195: System.Globalization.dll => 42
	i32 u0x3463c971, ; 196: System.Net.Http.Json => 64
	i32 u0x34a66c56, ; 197: lib_System.IO.Pipes.dll.so => 56
	i32 u0x352e5794, ; 198: lib_Xamarin.Google.ErrorProne.Annotations.dll.so => 265
	i32 u0x35e25008, ; 199: System.ComponentModel.Primitives.dll => 16
	i32 u0x3612ff2c, ; 200: lib_System.IO.dll.so => 58
	i32 u0x364e69a3, ; 201: System.IO.MemoryMappedFiles.dll => 53
	i32 u0x36e9595b, ; 202: lib_System.Transactions.dll.so => 151
	i32 u0x370eff4f, ; 203: lib_System.Globalization.Extensions.dll.so => 41
	i32 u0x373f6a31, ; 204: tr/Microsoft.Maui.Controls.resources.dll => 306
	i32 u0x3751ef41, ; 205: Xamarin.Google.Guava.ListenableFuture => 267
	i32 u0x3787b992, ; 206: lib_System.ComponentModel.DataAnnotations.dll.so => 14
	i32 u0x37ea9cd7, ; 207: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 236
	i32 u0x382704bd, ; 208: lib_Xamarin.AndroidX.Emoji2.ViewsHelper.dll.so => 220
	i32 u0x38c136f7, ; 209: System.Runtime.InteropServices.JavaScript.dll => 106
	i32 u0x38d89c1d, ; 210: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 226
	i32 u0x39481653, ; 211: lib_mscorlib.dll.so => 167
	i32 u0x399f1f06, ; 212: Xamarin.Google.Crypto.Tink.Android => 264
	i32 u0x39adca5e, ; 213: Xamarin.AndroidX.Lifecycle.Common.dll => 225
	i32 u0x3a20ecf3, ; 214: System.Diagnostics.Tracing => 34
	i32 u0x3a2aaa1d, ; 215: System.Xml.XDocument => 159
	i32 u0x3a8b0a79, ; 216: lib_Xamarin.KotlinX.Coroutines.Android.dll.so => 273
	i32 u0x3acd0267, ; 217: System.Private.DataContractSerialization.dll => 86
	i32 u0x3ad7b407, ; 218: System.Diagnostics.Tools => 32
	i32 u0x3b008d80, ; 219: lib_Xamarin.AndroidX.DynamicAnimation.dll.so => 218
	i32 u0x3b2c715c, ; 220: System.Collections.dll => 12
	i32 u0x3b3271e4, ; 221: zh-Hans/Microsoft.Maui.Controls.resources => 310
	i32 u0x3b458447, ; 222: lib_System.Threading.Tasks.Dataflow.dll.so => 142
	i32 u0x3b45fb35, ; 223: System.IO.FileSystem => 51
	i32 u0x3b4797e5, ; 224: es/Microsoft.Maui.Controls.resources => 284
	i32 u0x3bb6bd33, ; 225: System.IO.UnmanagedMemoryStream.dll => 57
	i32 u0x3c5e5b62, ; 226: Xamarin.AndroidX.SavedState.dll => 247
	i32 u0x3cbffa41, ; 227: System.Drawing => 36
	i32 u0x3d548d92, ; 228: Microsoft.Extensions.DependencyInjection.Abstractions => 178
	i32 u0x3d5a6611, ; 229: da/Microsoft.Maui.Controls.resources.dll => 281
	i32 u0x3d7be038, ; 230: Xamarin.Google.ErrorProne.Annotations.dll => 265
	i32 u0x3dbaaf8f, ; 231: Xamarin.AndroidX.AppCompat => 198
	i32 u0x3dc84a49, ; 232: System.Drawing.Primitives.dll => 35
	i32 u0x3df150e9, ; 233: lib_Xamarin.AndroidX.Interpolator.dll.so => 224
	i32 u0x3e444eb4, ; 234: System.Linq.Expressions.dll => 59
	i32 u0x3e5c42fd, ; 235: lib_System.Reflection.TypeExtensions.dll.so => 97
	i32 u0x3eb776a1, ; 236: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 235
	i32 u0x3ebd41f6, ; 237: lib_System.Collections.dll.so => 12
	i32 u0x3ecd3024, ; 238: lib_System.Resources.Reader.dll.so => 99
	i32 u0x3eea4db8, ; 239: lib_Microsoft.Extensions.Primitives.dll.so => 183
	i32 u0x3f3e1e33, ; 240: lib_Xamarin.AndroidX.Lifecycle.Process.dll.so => 230
	i32 u0x3f9dcf8c, ; 241: GoogleGson => 174
	i32 u0x408b17f4, ; 242: System.ComponentModel.TypeConverter => 17
	i32 u0x409e66d8, ; 243: Xamarin.Kotlin.StdLib => 270
	i32 u0x41761b2c, ; 244: System => 165
	i32 u0x4232ae7b, ; 245: lib_System.Reflection.Emit.dll.so => 93
	i32 u0x42be2972, ; 246: lib_System.Text.Encodings.Web.dll.so => 137
	i32 u0x42c091c1, ; 247: lib_Xamarin.Android.Glide.GifDecoder.dll.so => 192
	i32 u0x42da3e50, ; 248: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 233
	i32 u0x43362f15, ; 249: Microsoft.Extensions.Logging.Debug => 181
	i32 u0x4393e151, ; 250: lib-th-Microsoft.Maui.Controls.resources.dll.so => 305
	i32 u0x441f18e1, ; 251: lib_System.Security.Cryptography.OpenSsl.dll.so => 124
	i32 u0x444e5c8e, ; 252: lib_System.ComponentModel.TypeConverter.dll.so => 17
	i32 u0x44549c93, ; 253: lib_System.Net.WebProxy.dll.so => 79
	i32 u0x4474042c, ; 254: lib_System.Numerics.Vectors.dll.so => 83
	i32 u0x447dc2e6, ; 255: Xamarin.AndroidX.Window => 260
	i32 u0x44845810, ; 256: lib_System.Net.Http.dll.so => 65
	i32 u0x44c3958b, ; 257: lib_System.Private.DataContractSerialization.dll.so => 86
	i32 u0x45bde382, ; 258: lib_System.Windows.dll.so => 155
	i32 u0x45c677b2, ; 259: System.Web.dll => 154
	i32 u0x460b48eb, ; 260: Xamarin.AndroidX.VectorDrawable.Animated => 256
	i32 u0x463a8801, ; 261: Xamarin.AndroidX.Navigation.Runtime.dll => 242
	i32 u0x464305ed, ; 262: fi/Microsoft.Maui.Controls.resources => 285
	i32 u0x466ae52b, ; 263: lib_System.Threading.Overlapped.dll.so => 141
	i32 u0x47a87de7, ; 264: lib_System.Resources.Writer.dll.so => 101
	i32 u0x47b79c15, ; 265: pl/Microsoft.Maui.Controls.resources.dll => 298
	i32 u0x47c7b4fa, ; 266: Xamarin.AndroidX.Arch.Core.Common => 200
	i32 u0x480a69ad, ; 267: System.Diagnostics.Process => 29
	i32 u0x48aa6be3, ; 268: System.IO.IsolatedStorage => 52
	i32 u0x48bf92c4, ; 269: lib_Xamarin.AndroidX.Collection.dll.so => 204
	i32 u0x49654709, ; 270: lib_System.Threading.Timer.dll.so => 148
	i32 u0x499b8219, ; 271: nb/Microsoft.Maui.Controls.resources.dll => 296
	i32 u0x4a0189ae, ; 272: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 288
	i32 u0x4a18f6f7, ; 273: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 261
	i32 u0x4a4cd262, ; 274: Xamarin.AndroidX.Collection.Jvm.dll => 205
	i32 u0x4a8cb221, ; 275: lib_Xamarin.JSpecify.dll.so => 269
	i32 u0x4aaf6f7c, ; 276: Microsoft.Win32.Registry => 5
	i32 u0x4ae97402, ; 277: lib_Microsoft.Maui.Graphics.dll.so => 188
	i32 u0x4b275854, ; 278: Xamarin.KotlinX.Serialization.Core.Jvm => 277
	i32 u0x4b5eebe5, ; 279: Xamarin.AndroidX.Startup.StartupRuntime.dll => 251
	i32 u0x4b64b158, ; 280: Xamarin.KotlinX.Coroutines.Core.dll => 274
	i32 u0x4b863c7a, ; 281: lib_System.Private.Xml.Linq.dll.so => 88
	i32 u0x4b8a64a7, ; 282: Xamarin.AndroidX.VectorDrawable => 255
	i32 u0x4bb12d98, ; 283: lib_System.Runtime.Serialization.Xml.dll.so => 115
	i32 u0x4be46b58, ; 284: Xamarin.AndroidX.Collection.Ktx => 206
	i32 u0x4c071bea, ; 285: Xamarin.KotlinX.Coroutines.Android => 273
	i32 u0x4c3393c5, ; 286: Xamarin.AndroidX.Annotation.Jvm => 197
	i32 u0x4d14ee2b, ; 287: Xamarin.AndroidX.DrawerLayout.dll => 217
	i32 u0x4de0ce3b, ; 288: lib_Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll.so => 244
	i32 u0x4e08a30b, ; 289: System.Private.DataContractSerialization => 86
	i32 u0x4e98c997, ; 290: lib_Xamarin.AndroidX.Window.Extensions.Core.Core.dll.so => 261
	i32 u0x4ed70c83, ; 291: Xamarin.AndroidX.Window.dll => 260
	i32 u0x4eed2679, ; 292: System.Linq => 62
	i32 u0x4f97822f, ; 293: System.Runtime.Serialization.Json.dll => 113
	i32 u0x50255dd9, ; 294: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 289
	i32 u0x50acdfd7, ; 295: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 279
	i32 u0x514d38cd, ; 296: System.IO => 58
	i32 u0x52114ed3, ; 297: Xamarin.AndroidX.SavedState => 247
	i32 u0x523dc4c1, ; 298: System.Resources.ResourceManager => 100
	i32 u0x533678bd, ; 299: lib_System.Private.CoreLib.dll.so => 173
	i32 u0x53701274, ; 300: lib_System.IO.FileSystem.Watcher.dll.so => 50
	i32 u0x53936ab4, ; 301: System.Configuration.dll => 19
	i32 u0x53cefc50, ; 302: Xamarin.AndroidX.CoordinatorLayout => 210
	i32 u0x53f80ba6, ; 303: System.Runtime.Serialization.Formatters.dll => 112
	i32 u0x5423e47b, ; 304: System.Runtime.CompilerServices.Unsafe => 102
	i32 u0x54246761, ; 305: lib_System.Diagnostics.Tools.dll.so => 32
	i32 u0x5498bac9, ; 306: lib_Microsoft.VisualBasic.dll.so => 3
	i32 u0x54ca50cb, ; 307: System.Runtime.CompilerServices.VisualC => 103
	i32 u0x557217fe, ; 308: lib_System.Numerics.dll.so => 84
	i32 u0x557b5293, ; 309: System.Runtime.Handles => 105
	i32 u0x558bc221, ; 310: Xamarin.Google.Crypto.Tink.Android.dll => 264
	i32 u0x55ab7451, ; 311: Xamarin.AndroidX.Lifecycle.Common.Jvm => 226
	i32 u0x55d10363, ; 312: System.Net.Quic.dll => 72
	i32 u0x55dfaca3, ; 313: lib_Microsoft.Win32.Primitives.dll.so => 4
	i32 u0x55e55df2, ; 314: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 236
	i32 u0x568cd628, ; 315: System.Formats.Asn1.dll => 38
	i32 u0x569fcb36, ; 316: System.Diagnostics.Tools.dll => 32
	i32 u0x56c018af, ; 317: lib_System.IO.UnmanagedMemoryStream.dll.so => 57
	i32 u0x56e36530, ; 318: System.Runtime.Extensions.dll => 104
	i32 u0x56e7a7ad, ; 319: System.Net.Security.dll => 74
	i32 u0x5718a9ef, ; 320: System.Collections.Immutable.dll => 9
	i32 u0x57201017, ; 321: System.Security.Cryptography.OpenSsl => 124
	i32 u0x57261233, ; 322: System.IO.Compression.dll => 46
	i32 u0x57924923, ; 323: Xamarin.AndroidX.AppCompat.AppCompatResources => 199
	i32 u0x57a5e912, ; 324: Microsoft.Extensions.Primitives => 183
	i32 u0x5833866d, ; 325: System.Collections.Immutable => 9
	i32 u0x583e844f, ; 326: System.IO.Compression.Brotli.dll => 43
	i32 u0x58a57897, ; 327: Microsoft.Win32.Primitives => 4
	i32 u0x58cffa99, ; 328: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 248
	i32 u0x58fd6613, ; 329: hi/Microsoft.Maui.Controls.resources.dll => 288
	i32 u0x596b5b3a, ; 330: lib_System.Drawing.Primitives.dll.so => 35
	i32 u0x5a48cf6c, ; 331: el/Microsoft.Maui.Controls.resources.dll => 283
	i32 u0x5b9331b6, ; 332: System.Diagnostics.TextWriterTraceListener => 31
	i32 u0x5be451c7, ; 333: lib_Xamarin.AndroidX.Browser.dll.so => 202
	i32 u0x5bf8ca0f, ; 334: System.Text.RegularExpressions.dll => 139
	i32 u0x5bf975ce, ; 335: lib_BlockApp.App.dll.so => 0
	i32 u0x5bfdbb43, ; 336: System.Reflection.Emit.dll => 93
	i32 u0x5c680b40, ; 337: System.Reflection.Extensions.dll => 94
	i32 u0x5c7be408, ; 338: sk/Microsoft.Maui.Controls.resources.dll => 303
	i32 u0x5cabc9a4, ; 339: fr/Microsoft.Maui.Controls.resources => 286
	i32 u0x5d552ab7, ; 340: System.IO.FileSystem.Primitives => 49
	i32 u0x5d5a6c40, ; 341: System.Threading.Tasks.Dataflow.dll => 142
	i32 u0x5dccd455, ; 342: System.Runtime.Serialization.Json => 113
	i32 u0x5e0b6fdc, ; 343: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 277
	i32 u0x5e2d7514, ; 344: System.Threading.Overlapped => 141
	i32 u0x5e2e3abe, ; 345: lib_Microsoft.VisualBasic.Core.dll.so => 2
	i32 u0x5e33306d, ; 346: sv/Microsoft.Maui.Controls.resources => 304
	i32 u0x5e7321d2, ; 347: lib_System.ComponentModel.Primitives.dll.so => 16
	i32 u0x5ed5f779, ; 348: zh-Hant/Microsoft.Maui.Controls.resources => 311
	i32 u0x5ef2ee25, ; 349: System.Runtime.Serialization.dll => 116
	i32 u0x5f3ec4dd, ; 350: Xamarin.Google.ErrorProne.Annotations => 265
	i32 u0x5f6f0b5b, ; 351: System.Xml.Serialization => 158
	i32 u0x5f93db6e, ; 352: Microsoft.Maui.Controls.HotReload.Forms.dll => 313
	i32 u0x5fa7b851, ; 353: System.Net.WebClient => 77
	i32 u0x6078995d, ; 354: System.Net.WebSockets.Client.dll => 80
	i32 u0x60892624, ; 355: lib_System.Formats.Tar.dll.so => 39
	i32 u0x60b0136a, ; 356: Xamarin.AndroidX.Loader.dll => 239
	i32 u0x60b33958, ; 357: System.Dynamic.Runtime => 37
	i32 u0x60d97228, ; 358: Xamarin.AndroidX.ViewPager2 => 259
	i32 u0x60ec189c, ; 359: lib_Xamarin.AndroidX.Arch.Core.Runtime.dll.so => 201
	i32 u0x6176eff7, ; 360: Xamarin.AndroidX.Emoji2.ViewsHelper => 220
	i32 u0x6188ba7e, ; 361: Xamarin.AndroidX.CursorAdapter => 214
	i32 u0x61b9038d, ; 362: System.Net.Http.dll => 65
	i32 u0x61c036ca, ; 363: System.Text.RegularExpressions => 139
	i32 u0x61d59e0e, ; 364: System.ComponentModel.EventBasedAsync.dll => 15
	i32 u0x62021776, ; 365: lib_System.IO.Compression.dll.so => 46
	i32 u0x620a8774, ; 366: lib_System.Xml.ReaderWriter.dll.so => 157
	i32 u0x625755ef, ; 367: lib_WindowsBase.dll.so => 166
	i32 u0x62c6282e, ; 368: System.Runtime => 117
	i32 u0x62cec1a2, ; 369: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 275
	i32 u0x62d6c1e4, ; 370: Xamarin.AndroidX.Tracing.Tracing.dll => 253
	i32 u0x62d6ea10, ; 371: Xamarin.Google.Android.Material.dll => 262
	i32 u0x638b1991, ; 372: Xamarin.AndroidX.ConstraintLayout => 208
	i32 u0x63dee9da, ; 373: System.IO.FileSystem.DriveInfo.dll => 48
	i32 u0x63fca3d0, ; 374: System.Net.Primitives.dll => 71
	i32 u0x640c0103, ; 375: System.Net.WebSockets => 81
	i32 u0x641979dd, ; 376: Xamarin.JSpecify => 269
	i32 u0x641f3e5a, ; 377: System.Security.Cryptography => 127
	i32 u0x64d1e4f5, ; 378: System.Reflection.Metadata => 95
	i32 u0x6525abc9, ; 379: System.Security.Cryptography.Csp => 122
	i32 u0x654b1498, ; 380: lib_System.Transactions.Local.dll.so => 150
	i32 u0x656b7698, ; 381: System.Diagnostics.Debug.dll => 26
	i32 u0x6670b12e, ; 382: lib_System.Security.AccessControl.dll.so => 118
	i32 u0x66888819, ; 383: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 229
	i32 u0x66e27484, ; 384: System.Reflection.dll => 98
	i32 u0x66ffb0f8, ; 385: System.Diagnostics.FileVersionInfo.dll => 28
	i32 u0x6715dc86, ; 386: Xamarin.AndroidX.CardView.dll => 203
	i32 u0x67577fe1, ; 387: lib_System.Runtime.CompilerServices.VisualC.dll.so => 103
	i32 u0x677cd287, ; 388: ro/Microsoft.Maui.Controls.resources.dll => 301
	i32 u0x67fe8db2, ; 389: System.Transactions.Local.dll => 150
	i32 u0x68139a0d, ; 390: System.IO.Pipelines.dll => 54
	i32 u0x6816ab6a, ; 391: Mono.Android.Export => 170
	i32 u0x6853a83d, ; 392: Microsoft.Win32.Primitives.dll => 4
	i32 u0x68cc9d1e, ; 393: System.Resources.Reader.dll => 99
	i32 u0x68f61ae4, ; 394: lib_System.Formats.Asn1.dll.so => 38
	i32 u0x690d4b7d, ; 395: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 311
	i32 u0x69239124, ; 396: System.Diagnostics.TraceSource.dll => 33
	i32 u0x693efa35, ; 397: lib_System.Net.WebHeaderCollection.dll.so => 78
	i32 u0x6942234e, ; 398: System.Reflection.Extensions => 94
	i32 u0x6947f945, ; 399: Xamarin.AndroidX.SwipeRefreshLayout => 252
	i32 u0x6988f147, ; 400: Microsoft.Extensions.Logging.dll => 179
	i32 u0x69ae5451, ; 401: lib_System.Runtime.InteropServices.JavaScript.dll.so => 106
	i32 u0x69dc03cc, ; 402: System.Core.dll => 21
	i32 u0x69ec0683, ; 403: System.Globalization.Extensions.dll => 41
	i32 u0x69f4f41d, ; 404: lib_Xamarin.AndroidX.AppCompat.dll.so => 198
	i32 u0x6a216153, ; 405: Mono.Android.Runtime.dll => 171
	i32 u0x6a539b49, ; 406: lib_System.Runtime.Extensions.dll.so => 104
	i32 u0x6a96652d, ; 407: Xamarin.AndroidX.Fragment => 222
	i32 u0x6afaf338, ; 408: lib_System.Threading.dll.so => 149
	i32 u0x6b645ada, ; 409: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 286
	i32 u0x6bcd3296, ; 410: Xamarin.AndroidX.Loader => 239
	i32 u0x6be1e423, ; 411: nb/Microsoft.Maui.Controls.resources => 296
	i32 u0x6c111525, ; 412: Xamarin.Kotlin.StdLib.dll => 270
	i32 u0x6c13413e, ; 413: Xamarin.Google.Android.Material => 262
	i32 u0x6c5562e0, ; 414: lib_Xamarin.KotlinX.Coroutines.Core.dll.so => 274
	i32 u0x6c652ce8, ; 415: Xamarin.AndroidX.Navigation.UI.dll => 243
	i32 u0x6c687fa7, ; 416: Microsoft.VisualBasic.Core => 2
	i32 u0x6c96614d, ; 417: hu/Microsoft.Maui.Controls.resources => 290
	i32 u0x6cbab720, ; 418: System.Text.Encoding.Extensions => 135
	i32 u0x6cc30c8c, ; 419: System.Runtime.Serialization.Formatters => 112
	i32 u0x6cea70ab, ; 420: Microsoft.VisualStudio.DesignTools.TapContract => 315
	i32 u0x6cf3d432, ; 421: lib_Xamarin.AndroidX.VersionedParcelable.dll.so => 257
	i32 u0x6cff90ba, ; 422: Microsoft.Extensions.Logging.Abstractions.dll => 180
	i32 u0x6dcaebf7, ; 423: uk/Microsoft.Maui.Controls.resources.dll => 307
	i32 u0x6e1ed932, ; 424: Xamarin.Android.Glide.Annotations => 190
	i32 u0x6ec71a65, ; 425: System.Linq.Expressions => 59
	i32 u0x6f7a29e4, ; 426: System.Reflection.Primitives => 96
	i32 u0x6fab02f2, ; 427: lib_Xamarin.AndroidX.ConstraintLayout.dll.so => 208
	i32 u0x7009e4c3, ; 428: System.Formats.Tar.dll => 39
	i32 u0x705fa726, ; 429: Xamarin.AndroidX.Arch.Core.Runtime.dll => 201
	i32 u0x7068d361, ; 430: Microsoft.VisualStudio.DesignTools.TapContract.dll => 315
	i32 u0x7070c6c0, ; 431: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 310
	i32 u0x70972b6d, ; 432: System.Diagnostics.Contracts => 25
	i32 u0x70a66bdd, ; 433: System.Reflection.Metadata.dll => 95
	i32 u0x7124cf39, ; 434: System.Reflection.DispatchProxy => 90
	i32 u0x71490522, ; 435: System.Resources.ResourceManager.dll => 100
	i32 u0x71dc7c8b, ; 436: System.Collections.NonGeneric.dll => 10
	i32 u0x72fcebde, ; 437: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 199
	i32 u0x731dd955, ; 438: lib_Mono.Android.dll.so => 172
	i32 u0x739bd4a8, ; 439: System.Private.Xml.Linq => 88
	i32 u0x73b20433, ; 440: lib_System.IO.FileSystem.Primitives.dll.so => 49
	i32 u0x73fbecbe, ; 441: lib_System.Memory.dll.so => 63
	i32 u0x74126030, ; 442: lib_System.Net.WebClient.dll.so => 77
	i32 u0x74a1c5bb, ; 443: System.Resources.Writer => 101
	i32 u0x74d743bf, ; 444: ja/Microsoft.Maui.Controls.resources => 293
	i32 u0x74eee4ef, ; 445: Xamarin.AndroidX.Security.SecurityCrypto.dll => 249
	i32 u0x75533a5e, ; 446: Microsoft.Extensions.Configuration.dll => 175
	i32 u0x7593c33f, ; 447: lib_System.IO.FileSystem.AccessControl.dll.so => 47
	i32 u0x765c50a4, ; 448: Xamarin.Android.Glide.GifDecoder => 192
	i32 u0x77ec19b4, ; 449: System.Buffers.dll => 7
	i32 u0x781074ce, ; 450: hr/Microsoft.Maui.Controls.resources => 289
	i32 u0x784d3e49, ; 451: lib_System.Net.dll.so => 82
	i32 u0x785e97f1, ; 452: Xamarin.AndroidX.Lifecycle.ViewModel => 235
	i32 u0x78b622b1, ; 453: ar/Microsoft.Maui.Controls.resources.dll => 278
	i32 u0x790376c9, ; 454: lib_Xamarin.AndroidX.Annotation.Experimental.dll.so => 196
	i32 u0x791a414b, ; 455: Xamarin.Android.Glide => 189
	i32 u0x7970be4f, ; 456: lib-he-Microsoft.Maui.Controls.resources.dll.so => 287
	i32 u0x79d00016, ; 457: it/Microsoft.Maui.Controls.resources => 292
	i32 u0x79eb68ee, ; 458: System.Private.Xml => 89
	i32 u0x7a80bd4e, ; 459: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 228
	i32 u0x7aca0819, ; 460: System.Windows.dll => 155
	i32 u0x7b350579, ; 461: lib__Microsoft.Android.Resource.Designer.dll.so => 317
	i32 u0x7b473a37, ; 462: lib_Xamarin.AndroidX.Fragment.Ktx.dll.so => 223
	i32 u0x7b6f419e, ; 463: System.Diagnostics.TraceSource => 33
	i32 u0x7b8f6ff7, ; 464: lib_System.Runtime.Serialization.Json.dll.so => 113
	i32 u0x7bf8cdab, ; 465: System.Runtime.dll => 117
	i32 u0x7c51ebd4, ; 466: lib_System.Net.HttpListener.dll.so => 66
	i32 u0x7c9bf920, ; 467: System.Numerics.Vectors => 83
	i32 u0x7d065c82, ; 468: lib_Xamarin.Google.ErrorProne.TypeAnnotations.dll.so => 266
	i32 u0x7d702d52, ; 469: lib_System.Text.Encoding.dll.so => 136
	i32 u0x7e3cc7a5, ; 470: Microsoft.VisualStudio.DesignTools.XamlTapContract.dll => 316
	i32 u0x7ec9ffe9, ; 471: System.Console => 20
	i32 u0x7fb38cd2, ; 472: System.Collections.Specialized => 11
	i32 u0x7fc7a41e, ; 473: System.Xml.XmlSerializer.dll => 163
	i32 u0x7fd90a71, ; 474: lib_System.Text.Encoding.CodePages.dll.so => 134
	i32 u0x7fdcdc37, ; 475: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 294
	i32 u0x7ff65cf5, ; 476: Microsoft.VisualBasic.dll => 3
	i32 u0x802a7166, ; 477: lib_System.Diagnostics.FileVersionInfo.dll.so => 28
	i32 u0x8030853e, ; 478: ko/Microsoft.Maui.Controls.resources => 294
	i32 u0x8044e1bd, ; 479: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 295
	i32 u0x8081c489, ; 480: lib_Jsr305Binding.dll.so => 263
	i32 u0x80bd55ad, ; 481: Microsoft.Maui => 186
	i32 u0x80f2f56e, ; 482: lib_System.Runtime.Serialization.Formatters.dll.so => 112
	i32 u0x810c11c2, ; 483: ro/Microsoft.Maui.Controls.resources => 301
	i32 u0x8115bdf3, ; 484: lib_System.Resources.ResourceManager.dll.so => 100
	i32 u0x816751d8, ; 485: lib_System.Diagnostics.DiagnosticSource.dll.so => 27
	i32 u0x81a110ae, ; 486: lib_System.ComponentModel.EventBasedAsync.dll.so => 15
	i32 u0x820d22b3, ; 487: Microsoft.Extensions.Options.dll => 182
	i32 u0x82364da2, ; 488: lib_System.Buffers.dll.so => 7
	i32 u0x82a8237c, ; 489: Microsoft.Extensions.Logging.Abstractions => 180
	i32 u0x82b6c85e, ; 490: System.ObjectModel.dll => 85
	i32 u0x82bb5429, ; 491: lib_System.Linq.Expressions.dll.so => 59
	i32 u0x82c1cf3e, ; 492: lib_System.Net.Quic.dll.so => 72
	i32 u0x832ec206, ; 493: lib_System.Diagnostics.StackTrace.dll.so => 30
	i32 u0x83323b38, ; 494: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 275
	i32 u0x8334206b, ; 495: System.Net.Http => 65
	i32 u0x842e93b2, ; 496: Xamarin.AndroidX.VectorDrawable.Animated.dll => 256
	i32 u0x8471e4ec, ; 497: System.Threading.Tasks.Parallel => 144
	i32 u0x857e4dd2, ; 498: lib_System.Net.WebSockets.dll.so => 81
	i32 u0x8628f1a4, ; 499: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 302
	i32 u0x863c6ac5, ; 500: System.Xml.Serialization.dll => 158
	i32 u0x867c9c52, ; 501: System.Globalization.Extensions => 41
	i32 u0x86b0fd78, ; 502: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll.so => 237
	i32 u0x86bba59b, ; 503: lib_Microsoft.Maui.Controls.dll.so => 184
	i32 u0x8702d9a2, ; 504: System.Security.AccessControl.dll => 118
	i32 u0x871c9c1b, ; 505: Microsoft.Extensions.Configuration.Abstractions => 176
	i32 u0x872eeb7b, ; 506: Xamarin.Android.Glide.DiskLruCache.dll => 191
	i32 u0x875633cc, ; 507: fr/Microsoft.Maui.Controls.resources.dll => 286
	i32 u0x87a1a22b, ; 508: lib-it-Microsoft.Maui.Controls.resources.dll.so => 292
	i32 u0x87e25095, ; 509: Xamarin.AndroidX.RecyclerView.dll => 245
	i32 u0x87e7fdbb, ; 510: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 297
	i32 u0x881f94da, ; 511: lib_netstandard.dll.so => 168
	i32 u0x8873eb17, ; 512: th/Microsoft.Maui.Controls.resources => 305
	i32 u0x887ae6a1, ; 513: lib_Xamarin.AndroidX.Lifecycle.Runtime.Android.dll.so => 232
	i32 u0x88acefcd, ; 514: System.ServiceModel.Web.dll => 132
	i32 u0x88d8bfaa, ; 515: System.Net.Sockets => 76
	i32 u0x88ffe49e, ; 516: System.Net.Mail => 67
	i32 u0x896b7878, ; 517: System.Private.CoreLib.dll => 173
	i32 u0x8a068af2, ; 518: Xamarin.AndroidX.Annotation.dll => 195
	i32 u0x8a52059a, ; 519: System.Threading.Tasks.Parallel.dll => 144
	i32 u0x8b804dbf, ; 520: System.Runtime.InteropServices.RuntimeInformation.dll => 107
	i32 u0x8bbaa2cd, ; 521: System.ValueTuple => 152
	i32 u0x8c20c628, ; 522: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 285
	i32 u0x8c20f140, ; 523: lib_System.Console.dll.so => 20
	i32 u0x8c40e0db, ; 524: System.Net.Primitives => 71
	i32 u0x8d19e4a2, ; 525: lib_Xamarin.AndroidX.Lifecycle.LiveData.dll.so => 227
	i32 u0x8d24e767, ; 526: System.Xml.ReaderWriter.dll => 157
	i32 u0x8d3fac99, ; 527: tr/Microsoft.Maui.Controls.resources => 306
	i32 u0x8d52b2e2, ; 528: Microsoft.Extensions.Configuration => 175
	i32 u0x8d52d3de, ; 529: lib_System.Threading.Tasks.dll.so => 145
	i32 u0x8dc6dbce, ; 530: System.Security.Cryptography.Csp.dll => 122
	i32 u0x8dcb0101, ; 531: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 241
	i32 u0x8e02310f, ; 532: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 278
	i32 u0x8e114655, ; 533: System.Security.Principal.Windows.dll => 128
	i32 u0x8f24faee, ; 534: System.Web.HttpUtility => 153
	i32 u0x8f41c524, ; 535: Xamarin.AndroidX.Emoji2.dll => 219
	i32 u0x8f4e087a, ; 536: lib_System.Web.dll.so => 154
	i32 u0x8f8c64e2, ; 537: lib_System.Private.Xml.dll.so => 89
	i32 u0x8fa56e96, ; 538: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 314
	i32 u0x905355ed, ; 539: System.Threading.Tasks.Dataflow => 142
	i32 u0x905caa9d, ; 540: nl/Microsoft.Maui.Controls.resources => 297
	i32 u0x906d466b, ; 541: Xamarin.AndroidX.Collection.Ktx.dll => 206
	i32 u0x90e50509, ; 542: lib_System.Reflection.Primitives.dll.so => 96
	i32 u0x911615a7, ; 543: lib_Xamarin.AndroidX.Fragment.dll.so => 222
	i32 u0x912896e5, ; 544: System.Console.dll => 20
	i32 u0x9130f5e7, ; 545: System.ComponentModel.DataAnnotations.dll => 14
	i32 u0x91abdf3a, ; 546: lib_Xamarin.AndroidX.Startup.StartupRuntime.dll.so => 251
	i32 u0x924edee6, ; 547: System.Text.Encoding.dll => 136
	i32 u0x928c75ca, ; 548: System.Net.Sockets.dll => 76
	i32 u0x92916334, ; 549: System.Linq.Parallel.dll => 60
	i32 u0x92f50938, ; 550: Xamarin.AndroidX.ConstraintLayout.Core.dll => 209
	i32 u0x93554fdc, ; 551: netstandard.dll => 168
	i32 u0x93634cc0, ; 552: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll.so => 229
	i32 u0x93918882, ; 553: Java.Interop.dll => 169
	i32 u0x93dba8a1, ; 554: Microsoft.Maui.Controls => 184
	i32 u0x940d5c2f, ; 555: System.ComponentModel.EventBasedAsync => 15
	i32 u0x94147f61, ; 556: System.Net.ServicePoint => 75
	i32 u0x9438d78e, ; 557: lib_System.Text.Json.dll.so => 138
	i32 u0x9469ba86, ; 558: lib_Xamarin.AndroidX.Lifecycle.Runtime.dll.so => 231
	i32 u0x94798bc5, ; 559: System.AppContext.dll => 6
	i32 u0x94a1db18, ; 560: lib-id-Microsoft.Maui.Controls.resources.dll.so => 291
	i32 u0x94fad8e5, ; 561: lib_Xamarin.AndroidX.Activity.Ktx.dll.so => 194
	i32 u0x95178668, ; 562: System.Data.DataSetExtensions => 23
	i32 u0x955cf248, ; 563: Xamarin.AndroidX.Lifecycle.Runtime.dll => 231
	i32 u0x9593ae7f, ; 564: lib_Xamarin.AndroidX.SavedState.dll.so => 247
	i32 u0x963ac2da, ; 565: sk/Microsoft.Maui.Controls.resources => 303
	i32 u0x9659e17c, ; 566: Xamarin.Android.Glide.dll => 189
	i32 u0x96bea474, ; 567: lib_Microsoft.Maui.Controls.Xaml.dll.so => 185
	i32 u0x974b89a2, ; 568: System.Reflection.Emit.Lightweight.dll => 92
	i32 u0x98ba5a04, ; 569: Microsoft.CSharp => 1
	i32 u0x9930ee42, ; 570: System.Text.Encodings.Web => 137
	i32 u0x999dcf0d, ; 571: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android => 234
	i32 u0x99e2e424, ; 572: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 233
	i32 u0x99e370f2, ; 573: Xamarin.AndroidX.VectorDrawable.dll => 255
	i32 u0x9a1756ac, ; 574: System.Text.Encoding.Extensions.dll => 135
	i32 u0x9a20430d, ; 575: System.Net.Ping => 70
	i32 u0x9a5a3337, ; 576: System.Threading.ThreadPool => 147
	i32 u0x9b24ab96, ; 577: lib_System.Runtime.Serialization.Primitives.dll.so => 114
	i32 u0x9b500441, ; 578: Xamarin.KotlinX.Coroutines.Core.Jvm => 275
	i32 u0x9b5e5b1c, ; 579: lib_System.Diagnostics.Contracts.dll.so => 25
	i32 u0x9be14c08, ; 580: Xamarin.AndroidX.Fragment.Ktx => 223
	i32 u0x9bf052c1, ; 581: Microsoft.Extensions.Logging.Debug.dll => 181
	i32 u0x9bfe3a41, ; 582: System.Private.Xml.dll => 89
	i32 u0x9c165ff9, ; 583: System.Reflection.TypeExtensions.dll => 97
	i32 u0x9c375496, ; 584: Xamarin.AndroidX.CursorAdapter.dll => 214
	i32 u0x9c70e6c9, ; 585: Xamarin.AndroidX.DynamicAnimation => 218
	i32 u0x9c96ac4c, ; 586: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 243
	i32 u0x9c97ad4a, ; 587: System.Diagnostics.TextWriterTraceListener.dll => 31
	i32 u0x9cc03a58, ; 588: System.IO.Compression.ZipFile.dll => 45
	i32 u0x9cd341b2, ; 589: lib_System.Threading.Tasks.Parallel.dll.so => 144
	i32 u0x9cf12c56, ; 590: Xamarin.AndroidX.Lifecycle.LiveData => 227
	i32 u0x9e78dac1, ; 591: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 238
	i32 u0x9ec022c0, ; 592: Xamarin.Android.Glide.DiskLruCache => 191
	i32 u0x9ec4cf01, ; 593: System.Runtime.Loader => 110
	i32 u0x9ecf752a, ; 594: System.Xml.XDocument.dll => 159
	i32 u0x9ee22cc0, ; 595: System.Drawing.Primitives => 35
	i32 u0x9f3b757e, ; 596: Xamarin.KotlinX.Coroutines.Core => 274
	i32 u0x9f7ea921, ; 597: lib_System.Runtime.InteropServices.dll.so => 108
	i32 u0x9f8c6f40, ; 598: System.Data.Common.dll => 22
	i32 u0xa026a50c, ; 599: System.Runtime.Serialization.Xml.dll => 115
	i32 u0xa090e36a, ; 600: System.IO.dll => 58
	i32 u0xa0fb56af, ; 601: lib_System.Text.RegularExpressions.dll.so => 139
	i32 u0xa0ff7514, ; 602: Xamarin.AndroidX.Tracing.Tracing => 253
	i32 u0xa1d8b647, ; 603: System.Threading.Tasks.dll => 145
	i32 u0xa1fd7d9f, ; 604: System.Security.Claims => 119
	i32 u0xa21f5a1f, ; 605: System.Security.Cryptography.Cng.dll => 121
	i32 u0xa25c90e5, ; 606: lib_Xamarin.AndroidX.Core.dll.so => 211
	i32 u0xa262a30f, ; 607: System.Runtime.Numerics.dll => 111
	i32 u0xa2ce8457, ; 608: lib-es-Microsoft.Maui.Controls.resources.dll.so => 284
	i32 u0xa2e0939b, ; 609: Xamarin.AndroidX.Activity => 193
	i32 u0xa30769e5, ; 610: System.Threading.Channels => 140
	i32 u0xa32eb6f0, ; 611: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 199
	i32 u0xa35f8f92, ; 612: System.IO.Pipes.AccessControl => 55
	i32 u0xa3c818c7, ; 613: lib_System.Net.WebSockets.Client.dll.so => 80
	i32 u0xa3cc7fa7, ; 614: System.Runtime.InteropServices.JavaScript => 106
	i32 u0xa4672f3b, ; 615: Microsoft.Maui.Controls.Xaml => 185
	i32 u0xa493aa02, ; 616: lib_System.Collections.Concurrent.dll.so => 8
	i32 u0xa4caf7a7, ; 617: Microsoft.Maui.dll => 186
	i32 u0xa4d4aaf8, ; 618: lib_System.Security.Cryptography.Cng.dll.so => 121
	i32 u0xa4db22c6, ; 619: System.Text.Encoding.CodePages.dll => 134
	i32 u0xa4e79dfd, ; 620: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 236
	i32 u0xa522693c, ; 621: Xamarin.Jetbrains.Annotations.dll => 268
	i32 u0xa52ac270, ; 622: lib_Xamarin.AndroidX.Window.dll.so => 260
	i32 u0xa553c739, ; 623: lib_System.ValueTuple.dll.so => 152
	i32 u0xa5a0a402, ; 624: Xamarin.AndroidX.ViewPager.dll => 258
	i32 u0xa5b3182d, ; 625: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 246
	i32 u0xa5b67c07, ; 626: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 226
	i32 u0xa5c5753c, ; 627: lib_System.Collections.Immutable.dll.so => 9
	i32 u0xa5ea80d9, ; 628: lib_Xamarin.Android.Glide.Annotations.dll.so => 190
	i32 u0xa6133c7f, ; 629: lib_System.IO.FileSystem.DriveInfo.dll.so => 48
	i32 u0xa630ecdd, ; 630: Xamarin.AndroidX.Fragment.Ktx.dll => 223
	i32 u0xa668c988, ; 631: lib_System.Net.NameResolution.dll.so => 68
	i32 u0xa685bd50, ; 632: Xamarin.Google.ErrorProne.TypeAnnotations.dll => 266
	i32 u0xa7008e0b, ; 633: Microsoft.Maui.Graphics => 188
	i32 u0xa7042ae3, ; 634: uk/Microsoft.Maui.Controls.resources => 307
	i32 u0xa715dd7e, ; 635: System.Xml.XPath.XDocument.dll => 160
	i32 u0xa741ef0b, ; 636: es/Microsoft.Maui.Controls.resources.dll => 284
	i32 u0xa744f665, ; 637: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 242
	i32 u0xa78103bc, ; 638: Xamarin.AndroidX.CoordinatorLayout.dll => 210
	i32 u0xa8032c67, ; 639: lib_Microsoft.Win32.Registry.dll.so => 5
	i32 u0xa80db4e1, ; 640: System.Xml.dll => 164
	i32 u0xa81b119f, ; 641: lib_System.Security.Cryptography.dll.so => 127
	i32 u0xa8282c09, ; 642: System.ServiceProcess.dll => 133
	i32 u0xa8298928, ; 643: Xamarin.AndroidX.ResourceInspection.Annotation => 246
	i32 u0xa85a7b6c, ; 644: System.Xml.XmlDocument => 162
	i32 u0xa8c61dcb, ; 645: nl/Microsoft.Maui.Controls.resources.dll => 297
	i32 u0xa9379a4f, ; 646: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 237
	i32 u0xa9d96f9b, ; 647: System.Threading.Overlapped.dll => 141
	i32 u0xaa107fc4, ; 648: Xamarin.AndroidX.ViewPager => 258
	i32 u0xaa2b531f, ; 649: lib_System.Globalization.dll.so => 42
	i32 u0xaa36a797, ; 650: Xamarin.AndroidX.Transition => 254
	i32 u0xaa4e51ff, ; 651: el/Microsoft.Maui.Controls.resources => 283
	i32 u0xaa88e550, ; 652: Mono.Android.Export.dll => 170
	i32 u0xaa8a4878, ; 653: Microsoft.Maui.Essentials => 187
	i32 u0xab123e9a, ; 654: Xamarin.AndroidX.Activity.Ktx.dll => 194
	i32 u0xab5f85c3, ; 655: Jsr305Binding.dll => 263
	i32 u0xab606289, ; 656: System.Globalization.Calendars.dll => 40
	i32 u0xabbc23e8, ; 657: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 277
	i32 u0xabdea79a, ; 658: ru/Microsoft.Maui.Controls.resources => 302
	i32 u0xabf58099, ; 659: Xamarin.AndroidX.ExifInterface => 221
	i32 u0xac1dd496, ; 660: System.Net.dll => 82
	i32 u0xacd6baa9, ; 661: System.IO.UnmanagedMemoryStream => 57
	i32 u0xace3f9b4, ; 662: System.Dynamic.Runtime.dll => 37
	i32 u0xace7ba82, ; 663: lib_System.Security.Principal.Windows.dll.so => 128
	i32 u0xacf080de, ; 664: System.Reflection => 98
	i32 u0xad2a79b6, ; 665: mscorlib.dll => 167
	i32 u0xad6f1e8a, ; 666: System.Private.CoreLib => 173
	i32 u0xad90894d, ; 667: lib_Xamarin.KotlinX.Serialization.Core.dll.so => 276
	i32 u0xaddb6d38, ; 668: Xamarin.AndroidX.ViewPager2.dll => 259
	i32 u0xae037813, ; 669: System.Numerics.Vectors.dll => 83
	i32 u0xae1ce33f, ; 670: Xamarin.AndroidX.Annotation.Experimental.dll => 196
	i32 u0xaeb2d8a5, ; 671: lib_Microsoft.Extensions.Options.dll.so => 182
	i32 u0xaf06273c, ; 672: System.Resources.Reader => 99
	i32 u0xaf3a6b91, ; 673: lib_System.Diagnostics.Debug.dll.so => 26
	i32 u0xaf4af872, ; 674: System.Diagnostics.StackTrace.dll => 30
	i32 u0xaf624531, ; 675: System.Xml.XPath.XDocument => 160
	i32 u0xaf8b1081, ; 676: lib_Xamarin.AndroidX.SavedState.SavedState.Ktx.dll.so => 248
	i32 u0xb0682092, ; 677: System.ComponentModel.dll => 18
	i32 u0xb0ed41f3, ; 678: System.Security.Principal.Windows => 128
	i32 u0xb128f886, ; 679: System.Security.Cryptography.Algorithms.dll => 120
	i32 u0xb18af942, ; 680: Xamarin.AndroidX.DrawerLayout => 217
	i32 u0xb1a434a2, ; 681: lib_System.Xml.Linq.dll.so => 156
	i32 u0xb1a7d210, ; 682: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll.so => 234
	i32 u0xb21220a3, ; 683: Xamarin.AndroidX.Security.SecurityCrypto => 249
	i32 u0xb223fa8c, ; 684: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 280
	i32 u0xb28cab85, ; 685: lib_Xamarin.Android.Glide.DiskLruCache.dll.so => 191
	i32 u0xb294d40b, ; 686: lib_System.Net.Ping.dll.so => 70
	i32 u0xb2a03f9f, ; 687: Xamarin.AndroidX.Lifecycle.Process.dll => 230
	i32 u0xb3d3821c, ; 688: Xamarin.AndroidX.Startup.StartupRuntime => 251
	i32 u0xb434b64b, ; 689: WindowsBase.dll => 166
	i32 u0xb514b305, ; 690: _Microsoft.Android.Resource.Designer.dll => 317
	i32 u0xb58d85d9, ; 691: lib_System.Runtime.Handles.dll.so => 105
	i32 u0xb62a9ccb, ; 692: Xamarin.AndroidX.SavedState.SavedState.Ktx => 248
	i32 u0xb63fa9f0, ; 693: Xamarin.AndroidX.Navigation.Common => 240
	i32 u0xb6490b5e, ; 694: lib_Mono.Android.Export.dll.so => 170
	i32 u0xb65adef9, ; 695: Mono.Android.Runtime => 171
	i32 u0xb660be12, ; 696: System.ComponentModel.Primitives => 16
	i32 u0xb6a153b2, ; 697: lib_Xamarin.AndroidX.ViewPager2.dll.so => 259
	i32 u0xb70c6fb4, ; 698: lib_Xamarin.AndroidX.VectorDrawable.dll.so => 255
	i32 u0xb755818f, ; 699: System.Threading.Tasks => 145
	i32 u0xb76be845, ; 700: hu/Microsoft.Maui.Controls.resources.dll => 290
	i32 u0xb7e7c341, ; 701: lib_System.Globalization.Calendars.dll.so => 40
	i32 u0xb838e2b0, ; 702: System.Security.Cryptography.X509Certificates.dll => 126
	i32 u0xb8c22b7f, ; 703: System.Security.Claims.dll => 119
	i32 u0xb8fd311b, ; 704: System.Formats.Asn1 => 38
	i32 u0xb979e222, ; 705: System.Runtime.Serialization => 116
	i32 u0xba0dbf1c, ; 706: System.IO.FileSystem.AccessControl.dll => 47
	i32 u0xba4127cb, ; 707: System.Threading.Tasks.Extensions => 143
	i32 u0xbaa520e7, ; 708: lib_System.ObjectModel.dll.so => 85
	i32 u0xbab301d1, ; 709: System.Security.AccessControl => 118
	i32 u0xbb95ee37, ; 710: System.Diagnostics.Tracing.dll => 34
	i32 u0xbba64c02, ; 711: GoogleGson.dll => 174
	i32 u0xbc4c6465, ; 712: System.Reflection.Primitives.dll => 96
	i32 u0xbc652da7, ; 713: System.IO.MemoryMappedFiles => 53
	i32 u0xbc98c93d, ; 714: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 205
	i32 u0xbcc610a0, ; 715: lib_System.Reflection.Metadata.dll.so => 95
	i32 u0xbd113355, ; 716: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 240
	i32 u0xbd78b0c8, ; 717: Xamarin.AndroidX.Navigation.Fragment.dll => 241
	i32 u0xbddce8a2, ; 718: lib_System.Security.Principal.dll.so => 129
	i32 u0xbe3f07c2, ; 719: lib_System.Runtime.CompilerServices.Unsafe.dll.so => 102
	i32 u0xbe4755f4, ; 720: System.Security.SecureString => 130
	i32 u0xbe592c0c, ; 721: System.Web => 154
	i32 u0xbefef58f, ; 722: System.Data.dll => 24
	i32 u0xbf506931, ; 723: System.Xml.XmlDocument.dll => 162
	i32 u0xbfc8f642, ; 724: Microsoft.VisualStudio.DesignTools.XamlTapContract => 316
	i32 u0xbff2e236, ; 725: System.Threading => 149
	i32 u0xc04c3c0a, ; 726: System.Runtime.Handles.dll => 105
	i32 u0xc095e070, ; 727: lib_Xamarin.AndroidX.Lifecycle.Common.dll.so => 225
	i32 u0xc10b79a7, ; 728: Xamarin.AndroidX.Core.ViewTree => 213
	i32 u0xc1c6ebf4, ; 729: System.Reflection.DispatchProxy.dll => 90
	i32 u0xc217efb6, ; 730: lib_Xamarin.AndroidX.ConstraintLayout.Core.dll.so => 209
	i32 u0xc235e84d, ; 731: Xamarin.AndroidX.CardView => 203
	i32 u0xc2a37b91, ; 732: System.Linq.Queryable.dll => 61
	i32 u0xc2a993fa, ; 733: System.Threading.Tasks.Extensions.dll => 143
	i32 u0xc3428433, ; 734: lib_System.Reflection.Emit.ILGeneration.dll.so => 91
	i32 u0xc35f7fa4, ; 735: System.Resources.Writer.dll => 101
	i32 u0xc37f65ce, ; 736: Microsoft.Win32.Registry.dll => 5
	i32 u0xc3888e16, ; 737: System.ComponentModel.Annotations.dll => 13
	i32 u0xc3ba1d80, ; 738: lib_System.Security.Cryptography.Csp.dll.so => 122
	i32 u0xc4251ff9, ; 739: System.Security.Cryptography.Encoding => 123
	i32 u0xc4684d9e, ; 740: lib_System.Security.Cryptography.Algorithms.dll.so => 120
	i32 u0xc4a8494a, ; 741: System.Text.Encoding => 136
	i32 u0xc4e76306, ; 742: System.Diagnostics.FileVersionInfo => 28
	i32 u0xc591efe9, ; 743: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 176
	i32 u0xc5b097e4, ; 744: System.Net.Requests.dll => 73
	i32 u0xc5b776df, ; 745: Xamarin.AndroidX.CustomView.dll => 215
	i32 u0xc5b79d28, ; 746: System.Data => 24
	i32 u0xc69f3b41, ; 747: lib_System.Data.dll.so => 24
	i32 u0xc71af05d, ; 748: Xamarin.AndroidX.Arch.Core.Runtime => 201
	i32 u0xc76e512c, ; 749: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 244
	i32 u0xc774da4f, ; 750: Xamarin.AndroidX.Navigation.Runtime => 242
	i32 u0xc7a3b0f0, ; 751: lib_Xamarin.AndroidX.Transition.dll.so => 254
	i32 u0xc7b797d0, ; 752: lib_Xamarin.AndroidX.Core.Core.Ktx.dll.so => 212
	i32 u0xc821fc10, ; 753: lib_System.ComponentModel.dll.so => 18
	i32 u0xc82afec1, ; 754: System.Text.Json => 138
	i32 u0xc8693088, ; 755: Xamarin.AndroidX.Activity.Ktx => 194
	i32 u0xc86c06e3, ; 756: Xamarin.AndroidX.Core => 211
	i32 u0xc8a662e9, ; 757: Java.Interop => 169
	i32 u0xc8d10307, ; 758: lib_System.Diagnostics.TraceSource.dll.so => 33
	i32 u0xc92a6809, ; 759: Xamarin.AndroidX.RecyclerView => 245
	i32 u0xca5de1fa, ; 760: System.Runtime.CompilerServices.Unsafe.dll => 102
	i32 u0xcae37e41, ; 761: System.Security.Cryptography.OpenSsl.dll => 124
	i32 u0xcaf7bd4b, ; 762: Xamarin.AndroidX.CustomView.PoolingContainer => 216
	i32 u0xcb5af55c, ; 763: lib_System.Reflection.Emit.Lightweight.dll.so => 92
	i32 u0xcc5af6ee, ; 764: Microsoft.Extensions.DependencyInjection.dll => 177
	i32 u0xcc6479a0, ; 765: System.Xml => 164
	i32 u0xcc7d82b4, ; 766: netstandard => 168
	i32 u0xcd1dd0db, ; 767: Xamarin.AndroidX.DynamicAnimation.dll => 218
	i32 u0xcd5a809f, ; 768: System.Formats.Tar => 39
	i32 u0xcdd8cd54, ; 769: lib_Xamarin.AndroidX.Emoji2.dll.so => 219
	i32 u0xce3fa116, ; 770: lib_System.Diagnostics.Process.dll.so => 29
	i32 u0xce70fda2, ; 771: hr/Microsoft.Maui.Controls.resources.dll => 289
	i32 u0xcef19b37, ; 772: System.ComponentModel.TypeConverter.dll => 17
	i32 u0xcf3163e6, ; 773: Mono.Android => 172
	i32 u0xcf663a21, ; 774: ru/Microsoft.Maui.Controls.resources.dll => 302
	i32 u0xcfa20c36, ; 775: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 252
	i32 u0xcfbaacae, ; 776: System.Text.Json.dll => 138
	i32 u0xcfd0c798, ; 777: System.Transactions => 151
	i32 u0xd0418592, ; 778: Xamarin.AndroidX.Concurrent.Futures.dll => 207
	i32 u0xd09a0c02, ; 779: BlockApp.App => 0
	i32 u0xd0f73226, ; 780: lib_Xamarin.KotlinX.AtomicFU.Jvm.dll.so => 272
	i32 u0xd128d608, ; 781: System.Xml.Linq => 156
	i32 u0xd1854eb4, ; 782: System.Security.dll => 131
	i32 u0xd2757232, ; 783: System.Configuration => 19
	i32 u0xd2ff69f1, ; 784: System.Net.HttpListener => 66
	i32 u0xd310c033, ; 785: lib_Xamarin.Jetbrains.Annotations.dll.so => 268
	i32 u0xd328ac54, ; 786: vi/Microsoft.Maui.Controls.resources => 308
	i32 u0xd4045e1b, ; 787: lib_System.dll.so => 165
	i32 u0xd404ddfe, ; 788: lib_System.Runtime.Intrinsics.dll.so => 109
	i32 u0xd432d20b, ; 789: System.Threading.Timer => 148
	i32 u0xd457e5c9, ; 790: lib_Microsoft.CSharp.dll.so => 1
	i32 u0xd47cb45a, ; 791: lib_Xamarin.AndroidX.Arch.Core.Common.dll.so => 200
	i32 u0xd496c3c3, ; 792: lib_Xamarin.AndroidX.ExifInterface.dll.so => 221
	i32 u0xd4d2575b, ; 793: System.IO.FileSystem.AccessControl => 47
	i32 u0xd505225a, ; 794: lib_System.Xml.XPath.dll.so => 161
	i32 u0xd622b752, ; 795: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 301
	i32 u0xd664cdf2, ; 796: de/Microsoft.Maui.Controls.resources => 282
	i32 u0xd6665034, ; 797: Xamarin.Android.Glide.GifDecoder.dll => 192
	i32 u0xd67a52b3, ; 798: System.Net.WebSockets.Client => 80
	i32 u0xd715a361, ; 799: System.Linq.dll => 62
	i32 u0xd7f95f5a, ; 800: da/Microsoft.Maui.Controls.resources => 281
	i32 u0xd804d57a, ; 801: System.Runtime.InteropServices.RuntimeInformation => 107
	i32 u0xd889aee8, ; 802: lib_System.Threading.Channels.dll.so => 140
	i32 u0xd8950487, ; 803: Xamarin.AndroidX.Annotation.Experimental => 196
	i32 u0xd8bba49d, ; 804: lib_Xamarin.AndroidX.RecyclerView.dll.so => 245
	i32 u0xd8dbab5d, ; 805: System.IO.FileSystem.Primitives.dll => 49
	i32 u0xd90e5f5a, ; 806: Xamarin.AndroidX.Lifecycle.LiveData.Core => 228
	i32 u0xd92e86f1, ; 807: Xamarin.KotlinX.Serialization.Core.dll => 276
	i32 u0xd930cda0, ; 808: Xamarin.AndroidX.Navigation.Fragment => 241
	i32 u0xd943a729, ; 809: System.ComponentModel.DataAnnotations => 14
	i32 u0xd96cf6f7, ; 810: pt-BR/Microsoft.Maui.Controls.resources => 299
	i32 u0xd9f65f5e, ; 811: lib-el-Microsoft.Maui.Controls.resources.dll.so => 283
	i32 u0xd9fdda56, ; 812: Microsoft.Extensions.Configuration.Abstractions.dll => 176
	i32 u0xda2f27df, ; 813: System.Net.NetworkInformation => 69
	i32 u0xda4773dd, ; 814: he/Microsoft.Maui.Controls.resources => 287
	i32 u0xdabf74ac, ; 815: lib_Xamarin.AndroidX.Annotation.Jvm.dll.so => 197
	i32 u0xdae8aa5e, ; 816: Mono.Android.dll => 172
	i32 u0xdb258bb2, ; 817: Microsoft.Maui.Controls.HotReload.Forms => 313
	i32 u0xdb7f7e5d, ; 818: Xamarin.AndroidX.Browser => 202
	i32 u0xdb9df1ce, ; 819: Xamarin.AndroidX.Concurrent.Futures => 207
	i32 u0xdbb50d93, ; 820: ms/Microsoft.Maui.Controls.resources => 295
	i32 u0xdc5370c5, ; 821: lib_System.Web.HttpUtility.dll.so => 153
	i32 u0xdc68940c, ; 822: zh-Hant/Microsoft.Maui.Controls.resources.dll => 311
	i32 u0xdc96bdf5, ; 823: System.Net.WebProxy.dll => 79
	i32 u0xdcefb51d, ; 824: Xamarin.AndroidX.Core.Core.Ktx.dll => 212
	i32 u0xdd864306, ; 825: System.Runtime.Intrinsics => 109
	i32 u0xdda814c6, ; 826: Xamarin.AndroidX.Annotation => 195
	i32 u0xde068c70, ; 827: Xamarin.AndroidX.Navigation.Common.dll => 240
	i32 u0xde7354ab, ; 828: System.Net.NameResolution => 68
	i32 u0xdecad304, ; 829: System.Net.Http.Json.dll => 64
	i32 u0xdf1b1ecd, ; 830: lib_System.ServiceProcess.dll.so => 133
	i32 u0xdf6f3870, ; 831: System.Diagnostics.DiagnosticSource => 27
	i32 u0xdf9a7f42, ; 832: System.Xml.XPath => 161
	i32 u0xdfd65a5d, ; 833: lib_System.Diagnostics.Tracing.dll.so => 34
	i32 u0xe05b6245, ; 834: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll => 234
	i32 u0xe12f62fc, ; 835: lib_System.Threading.ThreadPool.dll.so => 147
	i32 u0xe13414bb, ; 836: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 290
	i32 u0xe1a41194, ; 837: lib_System.Xml.XDocument.dll.so => 159
	i32 u0xe1ae15d6, ; 838: Xamarin.AndroidX.Collection => 204
	i32 u0xe1eea3e4, ; 839: lib_System.IO.Compression.ZipFile.dll.so => 45
	i32 u0xe1f0a5d8, ; 840: lib_Xamarin.AndroidX.ViewPager.dll.so => 258
	i32 u0xe2098b0b, ; 841: System.Collections.NonGeneric => 10
	i32 u0xe250cda6, ; 842: lib_Microsoft.Extensions.Logging.dll.so => 179
	i32 u0xe2513246, ; 843: lib_System.Runtime.Numerics.dll.so => 111
	i32 u0xe27c1b41, ; 844: lib_Xamarin.KotlinX.AtomicFU.dll.so => 271
	i32 u0xe2a3f2e8, ; 845: System.Collections.Specialized.dll => 11
	i32 u0xe34ee011, ; 846: lib_System.IO.Pipelines.dll.so => 54
	i32 u0xe3774f52, ; 847: lib_System.IO.MemoryMappedFiles.dll.so => 53
	i32 u0xe3a54a09, ; 848: System.Net.WebProxy => 79
	i32 u0xe3c7860c, ; 849: lib_System.Security.Claims.dll.so => 119
	i32 u0xe3df9d2b, ; 850: System.Security.Cryptography.dll => 127
	i32 u0xe4436460, ; 851: System.Numerics.dll => 84
	i32 u0xe4fab729, ; 852: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 178
	i32 u0xe52378b9, ; 853: System.Net.Mail.dll => 67
	i32 u0xe56ef253, ; 854: System.Runtime.InteropServices.dll => 108
	i32 u0xe625b819, ; 855: lib_Xamarin.AndroidX.CardView.dll.so => 203
	i32 u0xe6b14171, ; 856: System.Net.HttpListener.dll => 66
	i32 u0xe6ca3640, ; 857: lib_Xamarin.AndroidX.Collection.Ktx.dll.so => 206
	i32 u0xe6e179fa, ; 858: System.Security.Principal => 129
	i32 u0xe6f98713, ; 859: System.Security.Cryptography.Encoding.dll => 123
	i32 u0xe797fcc1, ; 860: System.Net.WebHeaderCollection.dll => 78
	i32 u0xe79e77a6, ; 861: Xamarin.AndroidX.Transition.dll => 254
	i32 u0xe7c9e2bd, ; 862: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 244
	i32 u0xe7dc15ff, ; 863: zh-Hans/Microsoft.Maui.Controls.resources.dll => 310
	i32 u0xe839deed, ; 864: System.Collections.Concurrent.dll => 8
	i32 u0xe843daa0, ; 865: Xamarin.AndroidX.Core.dll => 211
	i32 u0xe89260c1, ; 866: Microsoft.VisualBasic.Core.dll => 2
	i32 u0xe90fdb70, ; 867: Xamarin.AndroidX.Collection.Jvm => 205
	i32 u0xe92ace5f, ; 868: lib_System.Linq.Parallel.dll.so => 60
	i32 u0xe99f7d24, ; 869: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 306
	i32 u0xe9b2d35e, ; 870: System.IO.Compression.FileSystem.dll => 44
	i32 u0xe9b630ed, ; 871: Xamarin.AndroidX.VersionedParcelable.dll => 257
	i32 u0xea0092d6, ; 872: lib_System.Threading.Tasks.Extensions.dll.so => 143
	i32 u0xea213423, ; 873: System.Xml.ReaderWriter => 157
	i32 u0xea4780ec, ; 874: System.Security.Principal.dll => 129
	i32 u0xea4fb52e, ; 875: Xamarin.AndroidX.Navigation.UI => 243
	i32 u0xeab81858, ; 876: lib_Microsoft.Maui.Essentials.dll.so => 187
	i32 u0xeaf244cc, ; 877: lib_System.IO.Pipes.AccessControl.dll.so => 55
	i32 u0xeaf598f6, ; 878: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 180
	i32 u0xeb2ecede, ; 879: System.Data.DataSetExtensions.dll => 23
	i32 u0xeb5560c9, ; 880: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 107
	i32 u0xebac8bfe, ; 881: System.Text.Encoding.CodePages => 134
	i32 u0xebb0254b, ; 882: lib_System.Net.NetworkInformation.dll.so => 69
	i32 u0xebc66336, ; 883: Xamarin.AndroidX.AppCompat.dll => 198
	i32 u0xec05582d, ; 884: Xamarin.AndroidX.Lifecycle.Process => 230
	i32 u0xed1090ae, ; 885: lib_System.Net.Primitives.dll.so => 71
	i32 u0xed409aea, ; 886: th/Microsoft.Maui.Controls.resources.dll => 305
	i32 u0xed96d41f, ; 887: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 210
	i32 u0xedadd6e2, ; 888: he/Microsoft.Maui.Controls.resources.dll => 287
	i32 u0xedf6669b, ; 889: lib_System.Drawing.dll.so => 36
	i32 u0xee9f991d, ; 890: System.Diagnostics.Process.dll => 29
	i32 u0xeeefb9c8, ; 891: lib_System.Dynamic.Runtime.dll.so => 37
	i32 u0xef5e8475, ; 892: Xamarin.AndroidX.Annotation.Jvm.dll => 197
	i32 u0xefd01a89, ; 893: System.IO.Pipelines => 54
	i32 u0xeff49a63, ; 894: System.Memory => 63
	i32 u0xeff49c4a, ; 895: lib_System.Text.Encoding.Extensions.dll.so => 135
	i32 u0xf09122fc, ; 896: lib_System.IO.IsolatedStorage.dll.so => 52
	i32 u0xf0ba55d9, ; 897: lib_Microsoft.Maui.Controls.HotReload.Forms.dll.so => 313
	i32 u0xf121f953, ; 898: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 228
	i32 u0xf1304331, ; 899: Microsoft.Maui.Controls.Xaml.dll => 185
	i32 u0xf15cb56d, ; 900: Xamarin.KotlinX.Serialization.Core => 276
	i32 u0xf1676aaa, ; 901: lib-da-Microsoft.Maui.Controls.resources.dll.so => 281
	i32 u0xf1ad867b, ; 902: System.Reflection.Emit.ILGeneration => 91
	i32 u0xf27f60d1, ; 903: System.Private.Xml.Linq.dll => 88
	i32 u0xf29c5384, ; 904: id/Microsoft.Maui.Controls.resources => 291
	i32 u0xf2ce3c98, ; 905: System.Threading.dll => 149
	i32 u0xf2dd3fc4, ; 906: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 293
	i32 u0xf323e0a6, ; 907: lib_Xamarin.Kotlin.StdLib.dll.so => 270
	i32 u0xf33c42ef, ; 908: lib_Xamarin.AndroidX.VectorDrawable.Animated.dll.so => 256
	i32 u0xf37dde86, ; 909: BlockApp.App.dll => 0
	i32 u0xf3a16066, ; 910: lib_Xamarin.AndroidX.Lifecycle.ViewModel.dll.so => 235
	i32 u0xf40add04, ; 911: Microsoft.Maui.Essentials.dll => 187
	i32 u0xf42589bc, ; 912: lib_System.Security.Cryptography.X509Certificates.dll.so => 126
	i32 u0xf45985cf, ; 913: System.Drawing.dll => 36
	i32 u0xf462c30d, ; 914: System.Private.Uri => 87
	i32 u0xf479582c, ; 915: Xamarin.AndroidX.Emoji2 => 219
	i32 u0xf47b0a29, ; 916: lib_System.Configuration.dll.so => 19
	i32 u0xf48143e5, ; 917: pt/Microsoft.Maui.Controls.resources.dll => 300
	i32 u0xf5185c24, ; 918: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 300
	i32 u0xf53cb11d, ; 919: lib_System.Net.Security.dll.so => 74
	i32 u0xf5861a4f, ; 920: pl/Microsoft.Maui.Controls.resources => 298
	i32 u0xf5e94e90, ; 921: ms/Microsoft.Maui.Controls.resources.dll => 295
	i32 u0xf5f4f1f0, ; 922: Microsoft.Extensions.DependencyInjection => 177
	i32 u0xf5fdf056, ; 923: lib_Microsoft.Extensions.DependencyInjection.dll.so => 177
	i32 u0xf60736e2, ; 924: System.IO.FileSystem.Watcher => 50
	i32 u0xf6318da0, ; 925: System.AppContext => 6
	i32 u0xf73be021, ; 926: System.Reflection.Emit.ILGeneration.dll => 91
	i32 u0xf76edc75, ; 927: System.Core => 21
	i32 u0xf7e95c85, ; 928: System.Xml.XmlSerializer => 163
	i32 u0xf807b767, ; 929: System.Reflection.TypeExtensions => 97
	i32 u0xf83dd773, ; 930: System.IO.FileSystem.Watcher.dll => 50
	i32 u0xf8420da3, ; 931: lib_Microsoft.VisualStudio.DesignTools.TapContract.dll.so => 315
	i32 u0xf86129d4, ; 932: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 304
	i32 u0xf93ba7d4, ; 933: System.Runtime.Serialization.Primitives => 114
	i32 u0xf94a8f86, ; 934: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 238
	i32 u0xf951b10e, ; 935: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 314
	i32 u0xf97c5a99, ; 936: System.Security => 131
	i32 u0xfa21f6af, ; 937: System.Net.WebClient.dll => 77
	i32 u0xfa50891f, ; 938: lib_System.Linq.dll.so => 62
	i32 u0xfa6ae1e2, ; 939: lib_Xamarin.AndroidX.Annotation.dll.so => 195
	i32 u0xfb0af295, ; 940: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 309
	i32 u0xfb1dad5d, ; 941: System.Diagnostics.DiagnosticSource.dll => 27
	i32 u0xfbc4b67c, ; 942: lib_System.IO.Compression.Brotli.dll.so => 43
	i32 u0xfc0a7526, ; 943: Xamarin.KotlinX.AtomicFU.Jvm.dll => 272
	i32 u0xfc5f7d36, ; 944: pt/Microsoft.Maui.Controls.resources => 300
	i32 u0xfdaee526, ; 945: Xamarin.AndroidX.Core.Core.Ktx => 212
	i32 u0xfdd1b433, ; 946: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 237
	i32 u0xfdf2741f, ; 947: System.Buffers => 7
	i32 u0xfe42d509, ; 948: lib_Xamarin.AndroidX.Security.SecurityCrypto.dll.so => 249
	i32 u0xfea12dee, ; 949: Microsoft.Maui.Controls.dll => 184
	i32 u0xfecef6ea, ; 950: System.Runtime.Numerics => 111
	i32 u0xff912ee3, ; 951: lib_System.Xml.Serialization.dll.so => 158
	i32 u0xffd4917f, ; 952: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 238
	i32 u0xfffce3e8 ; 953: Xamarin.AndroidX.ExifInterface.dll => 221
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [954 x i32] [
	i32 69, i32 73, i32 68, i32 239, i32 163, i32 109, i32 178, i32 231,
	i32 267, i32 48, i32 81, i32 307, i32 146, i32 272, i32 186, i32 279,
	i32 30, i32 125, i32 188, i32 103, i32 10, i32 250, i32 61, i32 309,
	i32 267, i32 67, i32 108, i32 250, i32 140, i32 31, i32 78, i32 125,
	i32 13, i32 207, i32 204, i32 271, i32 193, i32 133, i32 252, i32 253,
	i32 308, i32 262, i32 152, i32 160, i32 64, i32 298, i32 308, i32 75,
	i32 309, i32 18, i32 316, i32 202, i32 44, i32 26, i32 181, i32 1,
	i32 213, i32 215, i32 60, i32 42, i32 296, i32 312, i32 92, i32 130,
	i32 189, i32 208, i32 148, i32 227, i32 224, i32 280, i32 23, i32 303,
	i32 55, i32 70, i32 193, i32 84, i32 264, i32 117, i32 293, i32 312,
	i32 225, i32 13, i32 292, i32 280, i32 132, i32 175, i32 110, i32 56,
	i32 314, i32 150, i32 75, i32 146, i32 63, i32 147, i32 246, i32 317,
	i32 166, i32 232, i32 123, i32 304, i32 209, i32 12, i32 222, i32 126,
	i32 153, i32 233, i32 171, i32 114, i32 167, i32 169, i32 165, i32 224,
	i32 87, i32 250, i32 116, i32 85, i32 291, i32 285, i32 183, i32 125,
	i32 6, i32 151, i32 288, i32 61, i32 179, i32 51, i32 104, i32 115,
	i32 40, i32 312, i32 269, i32 263, i32 213, i32 261, i32 121, i32 299,
	i32 174, i32 132, i32 11, i32 52, i32 21, i32 44, i32 76, i32 120,
	i32 232, i32 215, i32 220, i32 216, i32 94, i32 282, i32 82, i32 137,
	i32 257, i32 22, i32 200, i32 8, i32 51, i32 299, i32 271, i32 74,
	i32 279, i32 156, i32 273, i32 214, i32 155, i32 164, i32 93, i32 268,
	i32 45, i32 294, i32 282, i32 266, i32 110, i32 131, i32 130, i32 25,
	i32 190, i32 162, i32 73, i32 56, i32 46, i32 98, i32 182, i32 90,
	i32 216, i32 22, i32 229, i32 146, i32 87, i32 43, i32 161, i32 72,
	i32 217, i32 278, i32 3, i32 42, i32 64, i32 56, i32 265, i32 16,
	i32 58, i32 53, i32 151, i32 41, i32 306, i32 267, i32 14, i32 236,
	i32 220, i32 106, i32 226, i32 167, i32 264, i32 225, i32 34, i32 159,
	i32 273, i32 86, i32 32, i32 218, i32 12, i32 310, i32 142, i32 51,
	i32 284, i32 57, i32 247, i32 36, i32 178, i32 281, i32 265, i32 198,
	i32 35, i32 224, i32 59, i32 97, i32 235, i32 12, i32 99, i32 183,
	i32 230, i32 174, i32 17, i32 270, i32 165, i32 93, i32 137, i32 192,
	i32 233, i32 181, i32 305, i32 124, i32 17, i32 79, i32 83, i32 260,
	i32 65, i32 86, i32 155, i32 154, i32 256, i32 242, i32 285, i32 141,
	i32 101, i32 298, i32 200, i32 29, i32 52, i32 204, i32 148, i32 296,
	i32 288, i32 261, i32 205, i32 269, i32 5, i32 188, i32 277, i32 251,
	i32 274, i32 88, i32 255, i32 115, i32 206, i32 273, i32 197, i32 217,
	i32 244, i32 86, i32 261, i32 260, i32 62, i32 113, i32 289, i32 279,
	i32 58, i32 247, i32 100, i32 173, i32 50, i32 19, i32 210, i32 112,
	i32 102, i32 32, i32 3, i32 103, i32 84, i32 105, i32 264, i32 226,
	i32 72, i32 4, i32 236, i32 38, i32 32, i32 57, i32 104, i32 74,
	i32 9, i32 124, i32 46, i32 199, i32 183, i32 9, i32 43, i32 4,
	i32 248, i32 288, i32 35, i32 283, i32 31, i32 202, i32 139, i32 0,
	i32 93, i32 94, i32 303, i32 286, i32 49, i32 142, i32 113, i32 277,
	i32 141, i32 2, i32 304, i32 16, i32 311, i32 116, i32 265, i32 158,
	i32 313, i32 77, i32 80, i32 39, i32 239, i32 37, i32 259, i32 201,
	i32 220, i32 214, i32 65, i32 139, i32 15, i32 46, i32 157, i32 166,
	i32 117, i32 275, i32 253, i32 262, i32 208, i32 48, i32 71, i32 81,
	i32 269, i32 127, i32 95, i32 122, i32 150, i32 26, i32 118, i32 229,
	i32 98, i32 28, i32 203, i32 103, i32 301, i32 150, i32 54, i32 170,
	i32 4, i32 99, i32 38, i32 311, i32 33, i32 78, i32 94, i32 252,
	i32 179, i32 106, i32 21, i32 41, i32 198, i32 171, i32 104, i32 222,
	i32 149, i32 286, i32 239, i32 296, i32 270, i32 262, i32 274, i32 243,
	i32 2, i32 290, i32 135, i32 112, i32 315, i32 257, i32 180, i32 307,
	i32 190, i32 59, i32 96, i32 208, i32 39, i32 201, i32 315, i32 310,
	i32 25, i32 95, i32 90, i32 100, i32 10, i32 199, i32 172, i32 88,
	i32 49, i32 63, i32 77, i32 101, i32 293, i32 249, i32 175, i32 47,
	i32 192, i32 7, i32 289, i32 82, i32 235, i32 278, i32 196, i32 189,
	i32 287, i32 292, i32 89, i32 228, i32 155, i32 317, i32 223, i32 33,
	i32 113, i32 117, i32 66, i32 83, i32 266, i32 136, i32 316, i32 20,
	i32 11, i32 163, i32 134, i32 294, i32 3, i32 28, i32 294, i32 295,
	i32 263, i32 186, i32 112, i32 301, i32 100, i32 27, i32 15, i32 182,
	i32 7, i32 180, i32 85, i32 59, i32 72, i32 30, i32 275, i32 65,
	i32 256, i32 144, i32 81, i32 302, i32 158, i32 41, i32 237, i32 184,
	i32 118, i32 176, i32 191, i32 286, i32 292, i32 245, i32 297, i32 168,
	i32 305, i32 232, i32 132, i32 76, i32 67, i32 173, i32 195, i32 144,
	i32 107, i32 152, i32 285, i32 20, i32 71, i32 227, i32 157, i32 306,
	i32 175, i32 145, i32 122, i32 241, i32 278, i32 128, i32 153, i32 219,
	i32 154, i32 89, i32 314, i32 142, i32 297, i32 206, i32 96, i32 222,
	i32 20, i32 14, i32 251, i32 136, i32 76, i32 60, i32 209, i32 168,
	i32 229, i32 169, i32 184, i32 15, i32 75, i32 138, i32 231, i32 6,
	i32 291, i32 194, i32 23, i32 231, i32 247, i32 303, i32 189, i32 185,
	i32 92, i32 1, i32 137, i32 234, i32 233, i32 255, i32 135, i32 70,
	i32 147, i32 114, i32 275, i32 25, i32 223, i32 181, i32 89, i32 97,
	i32 214, i32 218, i32 243, i32 31, i32 45, i32 144, i32 227, i32 238,
	i32 191, i32 110, i32 159, i32 35, i32 274, i32 108, i32 22, i32 115,
	i32 58, i32 139, i32 253, i32 145, i32 119, i32 121, i32 211, i32 111,
	i32 284, i32 193, i32 140, i32 199, i32 55, i32 80, i32 106, i32 185,
	i32 8, i32 186, i32 121, i32 134, i32 236, i32 268, i32 260, i32 152,
	i32 258, i32 246, i32 226, i32 9, i32 190, i32 48, i32 223, i32 68,
	i32 266, i32 188, i32 307, i32 160, i32 284, i32 242, i32 210, i32 5,
	i32 164, i32 127, i32 133, i32 246, i32 162, i32 297, i32 237, i32 141,
	i32 258, i32 42, i32 254, i32 283, i32 170, i32 187, i32 194, i32 263,
	i32 40, i32 277, i32 302, i32 221, i32 82, i32 57, i32 37, i32 128,
	i32 98, i32 167, i32 173, i32 276, i32 259, i32 83, i32 196, i32 182,
	i32 99, i32 26, i32 30, i32 160, i32 248, i32 18, i32 128, i32 120,
	i32 217, i32 156, i32 234, i32 249, i32 280, i32 191, i32 70, i32 230,
	i32 251, i32 166, i32 317, i32 105, i32 248, i32 240, i32 170, i32 171,
	i32 16, i32 259, i32 255, i32 145, i32 290, i32 40, i32 126, i32 119,
	i32 38, i32 116, i32 47, i32 143, i32 85, i32 118, i32 34, i32 174,
	i32 96, i32 53, i32 205, i32 95, i32 240, i32 241, i32 129, i32 102,
	i32 130, i32 154, i32 24, i32 162, i32 316, i32 149, i32 105, i32 225,
	i32 213, i32 90, i32 209, i32 203, i32 61, i32 143, i32 91, i32 101,
	i32 5, i32 13, i32 122, i32 123, i32 120, i32 136, i32 28, i32 176,
	i32 73, i32 215, i32 24, i32 24, i32 201, i32 244, i32 242, i32 254,
	i32 212, i32 18, i32 138, i32 194, i32 211, i32 169, i32 33, i32 245,
	i32 102, i32 124, i32 216, i32 92, i32 177, i32 164, i32 168, i32 218,
	i32 39, i32 219, i32 29, i32 289, i32 17, i32 172, i32 302, i32 252,
	i32 138, i32 151, i32 207, i32 0, i32 272, i32 156, i32 131, i32 19,
	i32 66, i32 268, i32 308, i32 165, i32 109, i32 148, i32 1, i32 200,
	i32 221, i32 47, i32 161, i32 301, i32 282, i32 192, i32 80, i32 62,
	i32 281, i32 107, i32 140, i32 196, i32 245, i32 49, i32 228, i32 276,
	i32 241, i32 14, i32 299, i32 283, i32 176, i32 69, i32 287, i32 197,
	i32 172, i32 313, i32 202, i32 207, i32 295, i32 153, i32 311, i32 79,
	i32 212, i32 109, i32 195, i32 240, i32 68, i32 64, i32 133, i32 27,
	i32 161, i32 34, i32 234, i32 147, i32 290, i32 159, i32 204, i32 45,
	i32 258, i32 10, i32 179, i32 111, i32 271, i32 11, i32 54, i32 53,
	i32 79, i32 119, i32 127, i32 84, i32 178, i32 67, i32 108, i32 203,
	i32 66, i32 206, i32 129, i32 123, i32 78, i32 254, i32 244, i32 310,
	i32 8, i32 211, i32 2, i32 205, i32 60, i32 306, i32 44, i32 257,
	i32 143, i32 157, i32 129, i32 243, i32 187, i32 55, i32 180, i32 23,
	i32 107, i32 134, i32 69, i32 198, i32 230, i32 71, i32 305, i32 210,
	i32 287, i32 36, i32 29, i32 37, i32 197, i32 54, i32 63, i32 135,
	i32 52, i32 313, i32 228, i32 185, i32 276, i32 281, i32 91, i32 88,
	i32 291, i32 149, i32 293, i32 270, i32 256, i32 0, i32 235, i32 187,
	i32 126, i32 36, i32 87, i32 219, i32 19, i32 300, i32 300, i32 74,
	i32 298, i32 295, i32 177, i32 177, i32 50, i32 6, i32 91, i32 21,
	i32 163, i32 97, i32 50, i32 315, i32 304, i32 114, i32 238, i32 314,
	i32 131, i32 77, i32 62, i32 195, i32 309, i32 27, i32 43, i32 272,
	i32 300, i32 212, i32 237, i32 7, i32 249, i32 184, i32 111, i32 158,
	i32 238, i32 221
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 u0x0000000000000000, ; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx @ 1dcfb6f8779c33b6f768c996495cb90ecd729329"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
