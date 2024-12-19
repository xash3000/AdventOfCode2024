conv = {'>':1 + 0j, '<':-1+0j, '^': -1j, 'v': 1j} # map instructions to complex numbers
narrow_space, insts = open('input.txt').read().split('\n\n')
space = [''.join([c*2 if c in ['#','.','@'] else '[]' for c in line ] ).replace('@@','@.') for line in narrow_space.split('\n')]
w, h = len(space[0]), len(space)
space = {complex(x,y):space[y][x] for y in range(h) for x in range(w)}
p = [key for key in space if space[key] == '@'][0] #find the robot
space[p] = '.'

for d in [conv[c] for c in insts.replace('\n','')]: # process the movements
    if space[p + d] == '.': # keep moving
        p += d
    elif  space[p + d] in ['[',']'] and d in [-1+0j, 1+0j]: # try to move boxes left/right
        cnt = 1
        while space[p + cnt * d] in ['[',']']:# check how many boxes are in a row
            cnt += 1
        if space[p + cnt * d] == '.':# there is room to shift
            for i in range(cnt, 0, -1): # shift
                space[p + i * d] = space[p + (i-1) * d]
            p += d # move robot
    elif space[p + d] in ['[',']'] and d in [1j, -1j]:# move up/down
        front = [z for z in [p + d, p + d -1] if space[z] == '['] # keep track of left brackets of each box in the stack
        seen = set(front)
        move = True
        while move and len(front) > 0:
            l = front.pop()
            move = space[l + d] != '#' and space[l + d + 1 + 0j] != "#" # check whether we hit a wall
            new = [z for z in [l + d + complex(j,0) for j in range(-1,2)] if space[z] == '[']# check for any boxes this one could push
            seen = seen.union(set(new)) # record these boxes in the stack
            front += new # record these boxes to check for boxes they could push
        if move:
            # start from the boxes furthest from the robot, move them, and then work backwards towards the robot repeating
            for layer in range(abs(int( min([-d.imag * z.imag for z in seen]) - p.imag)), 0, -1):
                for z in [w for w in seen if w.imag == (p + layer * d).imag]:
                    # clear the space this box inhabited
                    space[z] = '.'
                    space[z + 1 + 0j] = '.'
                    # move the box
                    space[z + d] = '['
                    space[z + d + 1 + 0j] = ']'
            p += d # move the robot

print(int(sum([100 * z.imag + z.real for z in space if space[z] == '['])))