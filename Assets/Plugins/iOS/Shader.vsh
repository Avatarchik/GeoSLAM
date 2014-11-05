//
//  Copyright (c) 2014 13th lab. All rights reserved.
//

attribute lowp vec2 position;
attribute lowp vec2 tex;
varying lowp vec2 texCoord;

void main()
{
    texCoord = tex;
    gl_Position = vec4(position.xy, 0.0, 1.0);
}
