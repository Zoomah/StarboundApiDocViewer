﻿using Markdig;
using System.IO;

namespace StarboundApiDocs {
  class Renderer {
    public static string fromFile(string filename) {
      return TEMPLATE.Replace("<%CONTENT%>", Markdown.ToHtml(File.ReadAllText(filename)));
    }

    private static string TEMPLATE = @"<!doctype html>
<html><head>
	<style>
    .markdown-body {
      -ms-text-size-adjust: 100%;
      -webkit-text-size-adjust: 100%;
      color: #333;
      font-family: ""Helvetica Neue"", Helvetica, ""Segoe UI"", Arial, freesans, sans-serif, ""Apple Color Emoji"", ""Segoe UI Emoji"", ""Segoe UI Symbol"";
      font-size: 14px;
      line-height: 1.6;
      word-wrap: break-word;
    }

    .markdown-body a {
      background-color: transparent;
      -webkit-text-decoration-skip: objects;
    }

    .markdown-body a:active,
    .markdown-body a:hover {
      outline-width: 0;
    }

    .markdown-body strong {
      font-weight: inherit;
    }

    .markdown-body strong {
      font-weight: bolder;
    }

    .markdown-body h1 {
      font-size: 2em;
      margin: 0.67em 0;
    }

    .markdown-body img {
      border-style: none;
    }

    .markdown-body svg:not(:root) {
      overflow:
      hidden;
    }

    .markdown-body code,
    .markdown-body kbd,
    .markdown-body pre {
      font-family: monospace, monospace;
      font-size: 1em;
    }

    .markdown-body hr {
      box-sizing: content-box;
      height: 0;
      overflow: visible;
    }

    .markdown-body input {
      font: inherit;
      margin: 0;
    }

    .markdown-body input {
      overflow: visible;
    }

    .markdown-body button:-moz-focusring,
    .markdown-body[type = ""button""]:-moz-focusring,
    .markdown-body[type = ""reset""]:-moz-focusring,
    .markdown-body[type = ""submit""]:-moz-focusring {
      outline: 1px dotted ButtonText;
    }

    .markdown-body[type = ""checkbox""] {
      box-sizing: border-box;
      padding: 0;
    }

    .markdown-body table {
      border-spacing: 0;
      border-collapse: collapse;
    }

    .markdown-body td,
    .markdown-body th {
      padding: 0;
    }

    .markdown-body* {
      box-sizing: border-box;
    }

    .markdown-body input {
      font: 13px/1.4 Helvetica, arial, nimbussansl, liberationsans, freesans, clean, sans-serif, ""Apple Color Emoji"", ""Segoe UI Emoji"", ""Segoe UI Symbol"";
    }

    .markdown-body a {
      color: #4078c0;
      text-decoration: none;
    }

    .markdown-body a:hover,
    .markdown-body a:active {
      text-decoration: underline;
    }

    .markdown-body hr {
      height: 0;
      margin: 15px 0;
      overflow: hidden;
      background: transparent;
      border: 0;
      border-bottom: 1px solid #ddd;
    }

    .markdown-body hr::before {
      display: table;
      content: "";
    }

    .markdown-body hr::after {
      display: table;
      clear: both;
      content: "";
    }

    .markdown-body h1,
    .markdown-body h2,
    .markdown-body h3,
    .markdown-body h4,
    .markdown-body h5,
    .markdown-body h6 {
      margin-top: 0;
      margin-bottom: 0;
      line-height: 1.5;
    }

    .markdown-body h1 {
      font-size: 30px;
    }

    .markdown-body h2 {
      font-size: 21px;
    }

    .markdown-body h3 {
      font-size: 16px;
    }

    .markdown-body h4 {
      font-size: 14px;
    }

    .markdown-body h5 {
      font-size: 12px;
    }

    .markdown-body h6 {
      font-size: 11px;
    }

    .markdown-body p {
      margin-top: 0;
      margin-bottom: 10px;
    }

    .markdown-body blockquote {
      margin: 0;
    }

    .markdown-body ul,
    .markdown-body ol {
      padding-left: 0;
      margin-top: 0;
      margin-bottom: 0;
    }

    .markdown-body ol ol,
    .markdown-body ul ol {
      list-style-type: lower-roman;
    }

    .markdown-body ul ul ol,
    .markdown-body ul ol ol,
    .markdown-body ol ul ol,
    .markdown-body ol ol ol {
      list-style-type: lower-alpha;
    }

    .markdown-body dd {
      margin-left: 0;
    }

    .markdown-body code {
      font-family: Consolas, ""Liberation Mono"", Menlo, Courier, monospace;
      font-size: 12px;
    }

    .markdown-body pre {
      margin-top: 0;
      margin-bottom: 0;
      font: 12px Consolas, ""Liberation Mono"", Menlo, Courier, monospace;
    }

    .markdown-body.pl-0 {
      padding-left: 0 !important;
    }

    .markdown-body.pl-1 {
      padding-left: 3px !important;
    }

    .markdown-body.pl-2 {
      padding-left: 6px !important;
    }

    .markdown-body.pl-3 {
      padding-left: 12px !important;
    }

    .markdown-body.pl-4 {
      padding-left: 24px !important;
    }

    .markdown-body.pl-5 {
      padding-left: 36px !important;
    }

    .markdown-body.pl-6 {
      padding-left: 48px !important;
    }

    .markdown-body.form-select::-ms-expand {
      opacity: 0;
    }

    .markdown-body:before {
      display: table;
      content: "";
    }

    .markdown-body:after {
      display: table;
      clear: both;
      content: "";
    }

    .markdown-body>*:first-child {
      margin-top: 0 !important;
    }

    .markdown-body>*:last-child {
      margin-bottom: 0 !important;
    }

    .markdown-body a:not([href]) {
      color:
      inherit;
      text - decoration: none;
    }

    .markdown-body.anchor {
      display: inline-block;
      padding-right: 2px;
      margin-left: -18px;
    }

    .markdown-body.anchor:focus {
      outline: none;
    }

    .markdown-body h1,
    .markdown-body h2,
    .markdown-body h3,
    .markdown-body h4,
    .markdown-body h5,
    .markdown-body h6 {
      margin-top: 1em;
      margin-bottom: 16px;
      font-weight: bold;
      line-height: 1.4;
    }

    .markdown-body h1.octicon-link,
    .markdown-body h2 .octicon-link,
    .markdown-body h3 .octicon-link,
    .markdown-body h4 .octicon-link,
    .markdown-body h5 .octicon-link,
    .markdown-body h6 .octicon-link {
      color: #000;
      vertical - align: middle;
      visibility:
      hidden;
    }

    .markdown-body h1:hover.anchor,
    .markdown-body h2:hover.anchor,
    .markdown-body h3:hover.anchor,
    .markdown-body h4:hover.anchor,
    .markdown-body h5:hover.anchor,
    .markdown-body h6:hover.anchor {
      text-decoration: none;
    }

    .markdown-body h1:hover.anchor.octicon-link,
    .markdown-body h2:hover.anchor.octicon-link,
    .markdown-body h3:hover.anchor.octicon-link,
    .markdown-body h4:hover.anchor.octicon-link,
    .markdown-body h5:hover.anchor.octicon-link,
    .markdown-body h6:hover.anchor.octicon-link {
      visibility: visible;
    }

    .markdown-body h1 {
      padding-bottom: 0.3em;
      font-size: 2.25em;
      line-height: 1.2;
      border-bottom: 1px solid #eee;
    }

    .markdown-body h1.anchor {
      line-height: 1;
    }

    .markdown-body h2 {
      padding-bottom: 0.3em;
      font-size: 1.75em;
      line-height: 1.225;
      border-bottom: 1px solid #eee;
    }

    .markdown-body h2.anchor {
      line-height: 1;
    }

    .markdown-body h3 {
      font-size: 1.5em;
      line-height: 1.43;
    }

    .markdown-body h3.anchor {
      line-height: 1.2;
    }

    .markdown-body h4 {
      font-size: 1.25em;
    }

    .markdown-body h4.anchor {
      line-height: 1.2;
    }

    .markdown-body h5 {
      font-size: 1em;
    }

    .markdown-body h5.anchor {
      line-height: 1.1;
    }

    .markdown-body h6 {
      font-size: 1em;
      color: #777;
    }

    .markdown-body h6.anchor {
      line-height: 1.1;
    }

    .markdown-body p,
    .markdown-body blockquote,
    .markdown-body ul,
    .markdown-body ol,
    .markdown-body dl,
    .markdown-body table,
    .markdown-body pre {
      margin-top: 0;
      margin-bottom: 16px;
    }

    .markdown-body hr {
      height: 1px;
      padding: 0;
      margin: 16px 0;
      background-color: #e7e7e7;
      border: 0 none;
    }

    .markdown-body ul,
    .markdown-body ol {
      padding-left: 2em;
    }

    .markdown-body ul ul,
    .markdown-body ul ol,
    .markdown-body ol ol,
    .markdown-body ol ul {
      margin-top: 0;
      margin-bottom: 0;
    }

    .markdown-body li>p {
      margin-top: 16px;
    }

    .markdown-body dl {
      padding: 0;
    }

    .markdown-body dl dt {
      padding: 0;
      margin-top: 16px;
      font-size: 1em;
      font-style: italic;
      font-weight: bold;
    }

    .markdown-body dl dd {
      padding: 0 16px;
      margin-bottom: 16px;
    }

    .markdown-body blockquote {
      padding: 0 15px;
      color: #777;
      border-left: 4px solid #ddd;
    }

    .markdown-body blockquote>:first-child {
      margin-top: 0;
    }

    .markdown-body blockquote>:last-child {
      margin-bottom: 0;
    }

    .markdown-body table {
      display: block;
      width: 100%;
      overflow: auto;
      word-break: normal;
      word-break: keep-all;
    }

    .markdown-body table th {
      font-weight: bold;
    }

    .markdown-body table th,
    .markdown-body table td {
      padding: 6px 13px;
      border: 1px solid #ddd;
    }

    .markdown-body table tr {
      background-color: #fff;
      border-top: 1px solid #ccc;
    }

    .markdown-body table tr:nth-child(2n) {
      background - color: #f8f8f8;
    }

    .markdown-body img {
      max-width: 100%;
      box-sizing: content-box;
      background-color: #fff;
    }

    .markdown-body code {
      padding: 0;
      padding-top: 0.2em;
      padding-bottom: 0.2em;
      margin: 0;
      font-size: 85%;
      background-color: rgba(0,0,0,0.04);
      border-radius: 3px;
    }

    .markdown-body code:before,
    .markdown-body code:after {
      letter-spacing: -0.2em;
      content: ""\00a0"";
    }

    .markdown-body pre>code {
      padding: 0;
      margin: 0;
      font-size: 100%;
      word-break: normal;
      white-space: pre;
      background: transparent;
      border: 0;
    }

    .markdown-body.highlight {
      margin-bottom: 16px;
    }

    .markdown-body.highlight pre,
    .markdown-body pre {
      padding: 16px;
      overflow: auto;
      font-size: 85%;
      line-height: 1.45;
      background-color: #f7f7f7;
      border-radius: 3px;
    }

    .markdown-body.highlight pre {
      margin-bottom: 0;
      word-break: normal;
    }

    .markdown-body pre {
      word-wrap: normal;
    }

    .markdown-body pre code {
      display: inline;
      max-width: initial;
      padding: 0;
      margin: 0;
      overflow: initial;
      line-height: inherit;
      word-wrap: normal;
      background-color: transparent;
      border: 0;
    }

    .markdown-body pre code:before,
    .markdown-body pre code:after {
      content: normal;
    }

    .markdown-body kbd {
      display: inline-block;
      padding: 3px 5px;
      font-size: 11px;
      line-height: 10px;
      color: #555;
      vertical-align: middle;
      background-color: #fcfcfc;
      border: solid 1px #ccc;
      border-bottom-color: #bbb;
      border-radius: 3px;
      box-shadow: inset 0 -1px 0 #bbb;
    }

    .markdown-body.pl-c {
      color: #969896;
    }

    .markdown-body.pl-c1,
    .markdown-body.pl-s.pl-v {
      color: #0086b3;
    }

    .markdown-body.pl-e,
    .markdown-body.pl-en {
      color: #795da3;
    }

    .markdown-body.pl-s.pl-s1,
    .markdown-body.pl-smi {
      color: #333;
    }

    .markdown-body.pl-ent {
      color: #63a35c;
    }

    .markdown-body.pl-k {
      color: #a71d5d;
    }

    .markdown-body.pl-pds,
    .markdown-body.pl-s,
    .markdown-body.pl-s.pl-pse.pl-s1,
    .markdown-body.pl-sr,
    .markdown-body.pl-sr.pl-cce,
    .markdown-body.pl-sr.pl-sra,
    .markdown-body.pl-sr.pl-sre {
      color: #183691;
    }

    .markdown-body.pl-v {
      color: #ed6a43;
    }

    .markdown-body.pl-id {
      color: #b52a1d;
    }

    .markdown-body.pl-ii {
      background-color: #b52a1d;
      color: #f8f8f8;
    }

    .markdown-body.pl-sr.pl-cce {
      color: #63a35c;
      font-weight: bold;
    }

    .markdown-body.pl-ml {
      color: #693a17;
    }

    .markdown-body.pl-mh,
    .markdown-body.pl-mh.pl-en,
    .markdown-body.pl-ms {
      color: #1d3e81;
      font-weight: bold;
    }

    .markdown-body.pl-mq {
      color: #008080;
    }

    .markdown-body.pl-mi {
      color: #333;
      font-style: italic;
    }

    .markdown-body.pl-mb {
      color: #333;
      font-weight: bold;
    }

    .markdown-body.pl-md {
      background-color: #ffecec;
      color: #bd2c00;
    }

    .markdown-body.pl-mi1 {
      background-color: #eaffea;
      color: #55a532;
    }

    .markdown-body.pl-mdr {
      color: #795da3;
      font-weight: bold;
    }

    .markdown-body.pl-mo {
      color: #1d3e81;
    }

    .markdown-body kbd {
      display: inline-block;
      padding: 3px 5px;
      font: 11px Consolas, ""Liberation Mono"", Menlo, Courier, monospace;
      line-height: 10px;
      color: #555;
      vertical-align: middle;
      background-color: #fcfcfc;
      border: solid 1px #ccc;
      border-bottom-color: #bbb;
      border-radius: 3px;
      box-shadow: inset 0 -1px 0 #bbb;
    }

    .markdown-body.full-commit.btn-outline:not(:disabled):hover {
      color: #4078c0;
      border: 1px solid #4078c0;
    }

    .markdown-body :checked+.radio-label {
      position: relative;
      z-index: 1;
      border-color: #4078c0;
    }

    .markdown-body.octicon {
      display: inline-block;
      vertical-align: text-top;
      fill: currentColor;
    }

    .markdown-body.task-list-item {
      list-style-type: none;
    }

    .markdown-body.task-list-item+.task-list-item {
      margin-top: 3px;
    }

    .markdown-body.task-list-item input {
      margin: 0 0.2em 0.25em -1.6em;
      vertical-align: middle;
    }

    .markdown-body hr {
      border-bottom-color: #eee;
    }
	</style>
</head><body oncontextmenu=""return false;"">
	<div class=""markdown-body""><%CONTENT%></div>
</body>
</html>";
  }
}
