﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Attachments.Abstractions;
using Regira.Entities.Attachments.Extensions;
using Regira.Entities.Web.Attachments.Models;
using Regira.Entities.Web.Models;
using Regira.Web.Extensions;
using Regira.Web.IO;

namespace Regira.Entities.Web.Attachments;

[ApiController]
[Route("attachments")]
public class AttachmentController(IAttachmentService service, IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetFile([FromRoute] int id, bool inline = true)
    {
        var item = await service.Details(id);

        if (item == null)
        {
            return NotFound();
        }

        return this.File(item, inline);
    }
    [HttpPost]
    public virtual async Task<ActionResult<SaveResult<AttachmentDto>>> Save(IFormFile file)
    {
        var item = file.ToNamedFile().ToAttachment<int>();
        await service.Save(item);
        await service.SaveChanges();
        var savedModel = mapper.Map<AttachmentDto>(item);
        return Ok(savedModel);
    }
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<SaveResult<AttachmentDto>>> Save([FromRoute] int id, IFormFile file)
    {
        var item = file.ToNamedFile().ToAttachment<int>();
        item.Id = id;
        await service.Save(item);
        await service.SaveChanges();
        var savedModel = mapper.Map<AttachmentDto>(item);
        return Ok(savedModel);
    }
}