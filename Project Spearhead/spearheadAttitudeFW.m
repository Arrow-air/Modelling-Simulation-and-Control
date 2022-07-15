function [U] = spearheadAttitudeFW(Ref,pts,ptsGains,pqr,V)
  Eap = Ref - pts;
  ptsDotRef(1) = ptsGains(1)*Eap(1);
  ptsDotRef(2) = ptsGains(2)*Eap(2);
  ptsDotRef(3) = (9.81/V)*tan(Ref(1))*cos(Ref(2));

  pqrRef = inv([1,sin(pts(1))*tan(pts(2)),cos(pts(1))*tan(pts(2));
                0,cos(pts(1)),-sin(pts(1));
                0,sin(pts(1))/cos(pts(2)),cos(pts(1))/cos(pts(2))])*ptsDotRef';

  Ear =  pqrRef - pqr;

  U = [Ear,pqrRef];
  end
