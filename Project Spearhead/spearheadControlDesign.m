figure(1);
plot(out.InARate1.Time,out.InARate1.Data(:,1));
figure(2);
plot(out.InARate1.Data(:,1),out.OutARate.Data(:,2));
figure(3);
plot(out.InARate1.Data(:,1),out.OutARate.Data(:,3));
figure(4);
plot(out.InARate1.Data(:,1),out.OutARate.Data(:,4));

num = tf1.Numerator;
dnum = tf1.Denominator;

Gphi = tf(num,dnum);

step(Gphi);