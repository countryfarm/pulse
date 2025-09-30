namespace Marap.Pulse.Infrastructure.Persistence
{
  using System;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;

  public static class VogenMappingExtensions
  {
    public static PropertyBuilder<TVo> HasVogenConversion<TVo,TPrimitive>(
      this PropertyBuilder<TVo> builder,
      Func<TVo, TPrimitive> toProvider,
      Func<TPrimitive, TVo> toEntity)
      where TVo : struct
    {
      return builder.HasConversion(
        vo   => toProvider(vo),
        raw  => toEntity(raw));
    }

    public static PropertyBuilder<TVo?> HasVogenConversion<TVo,TPrimitive>(
      this PropertyBuilder<TVo?> builder,
      Func<TVo, TPrimitive> toProvider,
      Func<TPrimitive, TVo> toEntity)
      where TVo : struct
      where TPrimitive : struct
    {
      return builder.HasConversion(
        vo   => vo.HasValue 
                  ? (TPrimitive?)toProvider(vo.Value) 
                  : null,
        raw  => raw.HasValue 
                  ? (TVo?)toEntity(raw.Value) 
                  : null);
    }
  }
}
