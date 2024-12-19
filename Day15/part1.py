conv = {'>':1 + 0j, '<':-1+0j, '^': -1j, 'v': 1j}
space, insts = open('input.txt').read().split('\n\n')
space = space.split('\n')
w, h = len(space[0]), len(space)
space = {complex(x,y):space[y][x] for y in range(h) for x in range(w)}
p = [key for key in space if space[key] == '@'][0]# find the robot
space[p] = '.'

for d in [conv[c] for c in insts.replace('\n','')]:# process the instructions
    if space[p + d] == '.':
        p += d
    elif space[p + d] == 'O':
        cnt = 1
        while space[p + cnt * d] =='O': # see how many boxes there are in a row
            cnt += 1
        if space[p + cnt * d] == '.':# shift the boxes
            space[p + d] = '.'
            space[p + cnt * d] = 'O'
            p += d # move the robot

print(int(sum([100 * z.imag + z.real for z in space if space[z] == 'O'])))