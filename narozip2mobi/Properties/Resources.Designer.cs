﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace narozip2mobi.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("narozip2mobi.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;container version=&quot;1.0&quot; xmlns=&quot;urn:oasis:names:tc:opendocument:xmlns:container&quot;&gt;
        ///  &lt;rootfiles&gt;
        ///    &lt;rootfile full-path=&quot;item/standard.opf&quot; media-type=&quot;application/oebps-package+xml&quot;/&gt;
        ///  &lt;/rootfiles&gt;
        ///&lt;/container&gt;
        /// に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string container_xml {
            get {
                return ResourceManager.GetString("container.xml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   application/epub+zip に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string manifest {
            get {
                return ResourceManager.GetString("manifest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   @charset &quot;utf-8&quot;;
        ///
        ///html {
        ///    -webkit-writing-mode: vertical-rl;
        ///    -webkit-text-orientation: mixed;
        ///    -epub-writing-mode: vertical-rl;
        ///    -epub-text-orientation: mixed;
        ///    writing-mode: vertical-rl;
        ///    text-orientation: mixed;
        ///}
        ///
        ///h1 {
        ///    font-size: 120%;
        ///}
        ///
        ///h2 {
        ///    font-size: 110%;
        ///}
        ///
        ///section {
        ///    break-after: page;
        ///    page-break-after: always;
        ///}
        ///
        ///p {
        ///    margin-left: 0%;
        ///    margin-right: 0%;
        ///}
        ///
        ///span.tcy {
        ///    text-combine-upright: all;
        ///    -webkit-text-combine: ho [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string style_css {
            get {
                return ResourceManager.GetString("style_css", resourceCulture);
            }
        }
    }
}
