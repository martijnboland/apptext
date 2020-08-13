---
id: introduction
title: Introduction
---

AppText is a Content Management System for Applications. A hybrid between a headless Content Management System and a Translation Management System.

AppText is built with [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) and [React](https:reactjs.org). You can install it via [NuGet packages](https://www.nuget.org/packages?q=apptext) into any existing ASP.NET Core web application.

## Why?

Many custom software applications (web, mobile, native) are shipped with embedded content. Think of labels, tooltips or even complete pages with information. This content often needs to be localized in multiple languages.

Once an application is released, updating the embedded content can become a bit of burden. Even the smallest textual change often requires a new version that needs to be deployed by the developer of the application. The same when adding a new language or adding missing translations.

The primary goal of AppText is to enable content updates in applications without having to go through the entire process of deploying a new version of the application.

## Who should use it?

AppText is intended for .NET Core and JavaScript application developers who want an easy way of managing multi-language content for their applications and being able to delegate content management to non-developers.

## When not to use it

If you need a Content Management System for a regular web site or blog, it's better to use one of the established systems such as Wordpress or Ghost.