//
//  Copyright (c) 2014 13th lab. All rights reserved.
//

varying lowp vec2 texCoord;
uniform lowp sampler2D texture;

void main()
{
    gl_FragColor = texture2D(texture, texCoord);
}
