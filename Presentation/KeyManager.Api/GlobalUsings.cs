// Global using directives

global using AutoMapper;
global using KeyManager.Api.Middlewares;
global using KeyManager.Api.Security.Authorization;
global using KeyManager.Infrastructure;
global using KeyManager.Infrastructure.Repository;
global using KeyManager.Domain.DTOs;
global using KeyManager.Domain.Exceptions;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Newtonsoft.Json;
global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Net;
global using System.Threading.Tasks;
global using Microsoft.OpenApi.Models;
global using IConfigurationProvider = AutoMapper.IConfigurationProvider;
global using KeyManager.Domain.Interfaces;