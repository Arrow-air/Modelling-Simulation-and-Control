function n=npolterm(deg,dim)
n=prod(max(deg,dim)+1:deg+dim)/factorial(min(deg,dim));
